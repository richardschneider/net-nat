using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat.Pcp
{
    /// <summary>
    ///   Requests a dynamic outbound mapping.
    /// </summary>
    public class PeerRequest : MapRequest
    {
        /// <summary>
        ///   Remote peer's port for the mapping.
        /// </summary>
        /// <value>
        ///   Must not be zero.
        /// </value>
        public ushort PeerPort { get; set; }

        /// <summary>
        ///   Remote peer's IP address.
        /// </summary>
        /// <value>
        ///   From the perspective of the local peer.
        /// </value>
        public IPAddress PeerAddress { get; set; }

        /// <inheritdoc />
        public override void Write(NatWriter writer)
        {
            base.Write(writer);

            writer.WriteUInt16(PeerPort);
            writer.WriteUInt16(0); // reserved
            writer.WriteIPv6Address(PeerAddress);
        }
    }
}
