using System;
using System.Collections.Generic;
using System.IO;
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
                opcode |= 0x80;
            }
            writer.WriteByte(opcode);
        }

        /// <inheritdoc />
        public override void Read(NatReader reader)
        {
            Version = reader.ReadByte();
            var opcode = reader.ReadByte();
            Opcode = (Opcode)(opcode & 0x7f);
            IsResponse = (opcode & 0x80) == 0x80;
        }

        /// <summary>
        ///   Create a message from the specified datagram.
        /// </summary>
        /// <param name="datagram">
        ///   The byte array containing the message.
        /// </param>
        /// <returns>
        ///   A <see cref="Message"/> that represents the <paramref name="datagram"/>.
        /// </returns>
        public static Message Create(byte[] datagram)
        {
            if (datagram == null || datagram.Length < 4)
            {
                throw new FormatException("NAT-PCP datagram must be at least 4 bytes.");
            }

            using (var ms = new MemoryStream(datagram, false))
            {
                // Get the type of message.
                var reader = new NatReader(ms);
                var header = new Message();
                header.Read(reader);
                Message msg = null;
                if (header.IsResponse)
                {
                    switch (header.Opcode)
                    {
                        case Opcode.Announce:
                            msg = new Response(); // no specific response data
                            break;
                        case Opcode.Map:
                            msg = new MapResponse();
                            break;
                        case Opcode.Peer:
                            msg = new Response(); // TODO
                            break;
                        default:
                            throw new NotSupportedException($"NAT-PCP response with opcode {header.Opcode}.");
                    }
                }
                else
                {
                    switch (header.Opcode)
                    {
                        case Opcode.Announce:
                            msg = new AnnounceRequest();
                            break;
                        case Opcode.Map:
                            msg = new MapRequest();
                            break;
                        case Opcode.Peer:
                            msg = new PeerRequest();
                            break;
                        default:
                            throw new NotSupportedException($"NAT-PCP request with opcode {header.Opcode}.");
                    }
                }

                // Read the message data.
                reader.Position = 0;
                msg.Read(reader);

                return msg;
            }
        }

        /// <summary>
        ///   Create a specific message from the specified datagram.
        /// </summary>
        /// <typeparam name="T">
        ///   The type of <see cref="Message"/>.
        /// </typeparam>
        /// <param name="datagram">
        ///   The byte array containing the message.
        /// </param>
        /// <returns>
        ///   A specific <see cref="Message"/> that represents the <paramref name="datagram"/>.
        /// </returns>
        public static T Create<T>(byte[] datagram)
            where T: Message
        {
            return (T)Create(datagram);
        }

    }
}
