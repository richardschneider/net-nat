using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat.Pcp
{
    /// <summary>
    ///   A Port Control Protocol request message.
    /// </summary>
    public class Request : Message
    {
        /// <summary>
        ///   The requested lifetime of a operation.
        /// </summary>
        /// <value>
        ///   Resolution in seconds.
        /// </value>
        public TimeSpan RequestedLifetime { get; set; }

        /// <summary>
        ///   The IP address of the client sending the request.
        /// </summary>
        /// <value>
        ///   Either IPv4 or IPv6.
        /// </value>
        public IPAddress ClientAddress { get; set; }

        /// <inheritdoc />
        public override void Write(NatWriter writer)
        {
            base.Write(writer);

            writer.WriteUInt16(0); // reserved
            writer.WriteTimeSpan(RequestedLifetime);
            writer.WriteIPv6Address(ClientAddress);
        }

    }
}
