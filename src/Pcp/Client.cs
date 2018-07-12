using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
            var hello = new byte[8 + 16];
            hello[0] = ProtocolVersion;
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

    }
}
