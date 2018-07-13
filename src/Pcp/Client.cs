using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat.Pcp
{
    /// <summary>
    ///   Communicates with a NAT that speaks the Port Control Protocol.
    /// </summary>
    /// <remarks>
    ///  PCP allows applications to create mappings from an external IP
    ///  address, protocol, and port to an internal IP address, protocol, and
    ///  port. These mappings are required for successful inbound
    ///  communications destined to machines located behind a NAT or a
    ///  firewall.
    /// </remarks>
    /// <seealso href="https://tools.ietf.org/html/rfc6887">RFC 6887 - NAT Port Control Protocol</seealso>
    public class Client : NatClient
    {
        /// <summary>
        ///   The version of the NAT-PCP.
        /// </summary>
        public const int ProtocolVersion = 2;

        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

        /// <summary>
        ///   Creates a new instance of the <see cref="Client"/> class with the specified
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
            var hello = new AnnounceRequest
            {
                ClientAddress = LocalEndPoint.Address
            };
            try
            {
                var res = await SendAndReceiveAsync(hello);
                return res[0] == ProtocolVersion && res[3] == 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public override async Task<LeasedEndpoint> CreatePublicEndpointAsync(int port)
        {
            var map = new MapRequest
            {
                ClientAddress = LocalEndPoint.Address,
                InternalPort = (ushort)port,
                Nonce = GenerateNonce(),
                Protocol = 6, // TODO
                RequestedLifetime = TimeSpan.FromHours(1), // TODO
                // TODO: SuggestedExternalAdddress
                SuggestedExternalAdddress = LocalEndPoint.AddressFamily == AddressFamily.InterNetwork
                    ? IPAddress.Any
                    : IPAddress.IPv6Any,
                SuggestedExternalPort = (ushort)port
            };
            var res = await SendAndReceiveAsync(map);
            //return res[0] == ProtocolVersion && res[3] == 0;
            return null;
        }

        /// <inheritdoc />
        public override Task DeletePublicEndpointAsync(LeasedEndpoint endpoint)
        {
            throw new NotImplementedException();
        }

        byte[] GenerateNonce()
        {
            var nonce = new byte[Message.NonceLength];
            rng.GetBytes(nonce);
            return nonce;
        }

    }
}
