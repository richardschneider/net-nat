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
    ///   <see cref="NatClient.CreatePublicEndpointAsync"/> is used to create
    ///   a <see cref="Lease"/>.
    ///   <para>
    ///   A background task is created to renew the lease every 
    ///   <see cref="Lease.Lifetime"/> / 2.  Use <see cref="Dispose"/> to
    ///   cancel the task.
    ///   </para>
    /// </remarks>
    public class LeasedEndpoint : IPEndPoint, IDisposable
    {
        CancellationTokenSource renewalCancellation = new CancellationTokenSource();

        /// <summary>
        ///   Create a new instance of the <see cref="LeasedEndpoint"/> class
        ///   from the specified <see cref="Lease"/>.
        /// </summary>
        /// <param name="lease">
        ///   An agreement for a public endpoint.
        /// </param>
        /// <seealso cref="NatClient.CreatePublicEndpointAsync"/>
        public LeasedEndpoint(Lease lease)
            : base(lease.PublicAddress, lease.PublicPort)
        {
            Lease = lease;

            Renewal(renewalCancellation.Token);
        }

        /// <summary>
        ///   The lease agreement.
        /// </summary>
        public Lease Lease { get; private set; }

        /// <inheritdoc />
        public void Dispose()
        {
            if (renewalCancellation != null) {
                var cancel = renewalCancellation;
                renewalCancellation = null;
                cancel.Cancel();
                cancel.Dispose();
            }

            if (Lease != null)
            {
                var lease = Lease;
                Lease = null;
                if (lease.Nat != null)
                {
                    lease.Nat.DeletePublicEndpointAsync(lease); // do not wait for task to complete!
                }
            }
        }

        async void Renewal(CancellationToken cancel)
        {
            while (!cancel.IsCancellationRequested)
            {
                int nextRenewal = (int)Lease.Lifetime.TotalMilliseconds / 2;
                for (; nextRenewal >= 250; nextRenewal /= 2)
                {
                    Console.WriteLine($"renewing lease in {nextRenewal / 1000}s on {this}");
                    try
                    {
                        await Task.Delay(nextRenewal, cancel);
                        var nextLease = await Lease.Nat.RenewPublicEndpointAsync(Lease);
                        // TODO: Check for endpoint change
                        Lease = nextLease;
                        break;
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
            }

            Console.WriteLine($"lease expired on {this}");
            Dispose();
        }
    }
}
