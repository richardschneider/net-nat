using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat
{
    /// <summary>
    ///   Request that an external port is mapped to an internal port.
    /// </summary>
    public class PmpMapRequest : PmpMessage
    {
        /// <summary>
        ///   The local port on which the client is listening.
        /// </summary>
        public ushort InternalPort { get; set; }

        /// <summary>
        ///   Preferred external port.
        /// </summary>
        /// <value>
        ///   If zero, then NAT assigns a random port number.
        /// </value>
        /// <remarks>
        ///   The NAT can ignore the external port and assign it own value.
        /// </remarks>
        public ushort PreferredExternalPort { get; set; }

        /// <summary>
        ///   How long to reserve the mapping.
        /// </summary>
        /// <value>
        ///   Defaults to 2 hours.
        /// </value>
        public TimeSpan Lifetime { get; set; } = TimeSpan.FromHours(2);

        /// <inheritdoc />
        public override void Write(NatWriter writer)
        {
            base.Write(writer);
            writer.WriteUInt16(0); // reserved
            writer.WriteUInt16(InternalPort);
            writer.WriteUInt16(PreferredExternalPort);
            writer.WriteTimeSpan(Lifetime);
        }
    }
}
