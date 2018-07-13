using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat
{
    /// <summary>
    ///   A public endpoint that is managed by a NAT.
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
        /// <seealso cref="NatClient.CreatePublicEndpointAsync"/>
        public LeasedEndpoint(NatClient nat, IPAddress address, int port)
            : base(address, port)
        {
            this.nat = nat;
        }

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
