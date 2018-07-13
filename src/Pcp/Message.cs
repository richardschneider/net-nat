using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat.Pcp
{
    /// <summary>
    ///   A Port Control Protocol message.
    /// </summary>
    public class Message : NatMessage
    {
        /// <summary>
        ///   The size of a nonce in bytes.
        /// </summary>
        /// <value>
        ///   12 bytes.
        /// </value>
        public const int NonceLength = 96 / 8;

        /// <summary>
        ///   The protocol version.
        /// </summary>
        /// <value>
        ///   Defaults to zero, <see cref="Client.ProtocolVersion"/>.
        /// </value>
        public byte Version { get; set; } = Client.ProtocolVersion;

        /// <summary>
        ///   The operatation.
        /// </summary>
        /// <value>
        ///   One of the <see cref="Opcode"/> values.
        /// </value>
        public Opcode Opcode { get; set; }

        /// <summary>
        ///   Indicates that the message is a response.
        /// </summary>
        /// <value>
        ///   Defaults to <b>false</b>, e.g. its a request.
        /// </value>
        public bool IsResponse { get; set; }

        /// <inheritdoc />
        public override void Write(NatWriter writer)
        {
            writer.WriteByte(Version);
            byte opcode = (byte)Opcode;
            if (IsResponse)
            {
                opcode |= 80;
            }
            writer.WriteByte(opcode);
        }

    }
}
