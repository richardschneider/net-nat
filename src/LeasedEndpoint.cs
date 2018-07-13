using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat
{
    /// <summary>
    ///   A public endpoint that is rented from a NAT.
    /// </summary>
    /// <remarks>
    ///   <see cref="NatClient.CreatePublicEndpointAsync"/> should be used to construct
    ///   the <b>LeasedEndpoint</b>.
    /// </remarks>
    public class LeasedEndpoint : IPEndPoint, IDisposable
    {
        NatClient nat;

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
            if (nat != null)
            {
                var controller = nat;
                nat = null;
                controller.DeletePublicEndpointAsync(this);
            }
        }
    }
}
