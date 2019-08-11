using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat.Pmp
{
    /// <summary>
    ///   Communicates with a NAT that speaks the Port Mapping Protocol.
    /// </summary>
    /// <remarks>
    ///   Automates the process of creating Network Address Translation(NAT) port mappings.
    ///   Allows retrieving the external IPv4 address
    ///   of a NAT gateway, thus allowing a client to make its external IPv4
    ///   address and port known to peers that may wish to communicate with it.
    ///  <para>
    ///   NAT-PMP is  superseded by the IETF Standards Track RFC "Port Control Protocol
    ///   (PCP)", which builds on NAT-PMP and uses a compatible packet format,
    ///   but adds a number of significant enhancements.
    ///  </para>
    /// </remarks>
    /// <seealso href="https://tools.ietf.org/html/rfc6886">RFC 6886 - NAT Port Mapping Protocol</seealso>
    /// <seealso href="https://tools.ietf.org/html/rfc6887">RFC 6887 - NAT Port Control Protocol</seealso>
    public class Client : NatClient
    {
        /// <summary>
        ///   The version of the NAT-PMP.
        /// </summary>
        public const int ProtocolVersion = 0;

        /// <summary>
        ///   Creates a new instance of the NAT-PMP <see cref="Client"/> class with the specified
        ///   IP Address of the NAT.
        /// </summary>
        /// <param name="address">
        ///   The IP address of the NAT server.
        /// </param>
        public Client(IPAddress address) : base(address)
        {
        }

        /// <inheritdoc />
        public override async Task<bool> IsAvailableAsync()
        {
            var hello = new ExternalAddressRequest();
            try
            {
                var res = await SendAndReceiveAsync(hello);
                if (res[0] != ProtocolVersion)
                    return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public override Task<Lease> CreatePublicEndpointAsync(int port)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Task<Lease> RenewPublicEndpointAsync(Lease lease)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Task DeletePublicEndpointAsync(Lease lease)
        {
            throw new NotImplementedException();
        }

    }
}

