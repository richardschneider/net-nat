using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat
{
    /// <summary>
    ///   A Port Control Protocol message.
    /// </summary>
    public class PcpMessage : PmpMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="PcpMessage"/> class.
        /// </summary>
        /// <remarks>
        ///   Set <see cref="Version"/> to <see cref="NatPcp.ProtocolVersion"/>.
        /// </remarks>
        public PcpMessage()
        {
            Version = NatPcp.ProtocolVersion;
        }

    }
}
