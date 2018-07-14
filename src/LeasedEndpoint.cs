using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Makaretu.Nat
{
    /// <summary>
    ///   A public endpoint that is rented from a NAT.
    /// </summary>
    /// <remarks>
    ///   <see cref="NatClient.CreatePublicEndpointAsync"/> should be used to construct
    ///   the <b>LeasedEndpoint</b>.
    ///   <para>
    ///   The lease is renewed in <see cref="Lifetime"/> / 2.
    ///   </para>
    /// </remarks>
    public class LeasedEndpoint : IPEndPoint, IDisposable
    {
        NatClient nat;
        CancellationTokenSource renewalCancellation = new CancellationTokenSource();

        /// <summary>
        ///   Create a new instance of the <see cref="LeasedEndpoint"/> class.
        /// </summary>
        /// <param name="nat">
        ///   The NAT that owns the public endpoint.
        /// </param>
        /// <param name="address">
        ///   The public IP address.
        /// </param>
        /// <param name="port">
        ///   The public port.
        /// </param>
        /// <param name="internalPort">
        ///   The host local port.
        /// </param>
        /// <param name="lifetime">
        ///   The leased time.
        /// </param>
        /// <seealso cref="NatClient.CreatePublicEndpointAsync"/>
        public LeasedEndpoint(NatClient nat, IPAddress address, int port, int internalPort, TimeSpan lifetime)
            : base(address, port)
        {
            this.nat = nat;
            InternalPort = internalPort;
            Lifetime = lifetime;
            
            Renewal(renewalCancellation.Token);
        }

        /// <summary>
        ///   The internal port for the public endpoint.
        /// </summary>
        public int InternalPort { get; private set; }

        /// <summary>
        ///   The lifetime of the leased public endpoint.
        /// </summary>
        public TimeSpan Lifetime { get; set; }

        /// <inheritdoc />
        public void Dispose()
        {
            if (renewalCancellation != null) {
                var cancel = renewalCancellation;
                renewalCancellation = null;
                cancel.Cancel();
                cancel.Dispose();
            }

            if (nat != null)
            {
                var controller = nat;
                nat = null;
                controller.DeletePublicEndpointAsync(this); // do not wait for task to complete!
            }
        }

        async void Renewal(CancellationToken cancel)
        {
            int nextRenewal = (int)Lifetime.TotalMilliseconds / 2;

            for (; nextRenewal >= 250; nextRenewal /= 2)
            {
                Console.WriteLine($"renewing lease in {nextRenewal}ms on {this}");
                try
                {
                    await Task.Delay(nextRenewal, cancel);
                    // TODO: renew the lease
                }
                catch (TaskCanceledException) 
                {
                    Console.WriteLine($"cancelled lease on {this}");
                    return;
                }
                catch (Exception)
                {
                    // keep on trucking
                }
            }

            Console.WriteLine($"lease expired on {this}");
            Dispose();
        }
    }
}
