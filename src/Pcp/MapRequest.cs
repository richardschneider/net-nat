using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat.Pcp
{
    /// <summary>
    ///   Requests a public (external) end point that can be connected to by
    ///   devices outside of the NAT.
    /// </summary>
    public class MapRequest : Request
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="MapRequest"/> class.
        /// </summary>
        public MapRequest()
        {
            Opcode = Opcode.Map;
        }

        /// <summary>
        ///   A randome value.
        /// </summary>
        /// <value>
        ///   Must be 96 bits (12 byte).
        /// </value>
        public byte[] Nonce { get; set; }

        /// <summary>
        ///   Upper-layer protocol associated with this Opcode.
        /// </summary>
        /// <value>
        ///   One of the IANA assigned protocol values.  TCP=6 and UDP=17.  Zero, the default,
        ///   indicates all protocols.
        /// </value>
        /// <seealso href="https://www.iana.org/assignments/protocol-numbers/protocol-numbers.xhtml"/>
        public byte Protocol { get; set; }

        /// <summary>
        ///   Internal port for the mapping.
        /// </summary>
        /// <value>
        ///   The value 0 indicates 'all ports', and is legal when the lifetime is zero (a delete
        ///   request), if the protocol does not use 16-bit port numbers, or the
        ///   client is requesting 'all ports'.  If the protocol is zero
        ///   (meaning 'all protocols'), then internal port MUST be zero on
        ///   transmission and MUST be ignored on reception.
        /// </value>
        public ushort InternalPort { get; set; }

        /// <summary>
        ///   Suggested external port for the mapping.
        /// </summary>
        /// <value>
        ///   This is useful for refreshing a mapping, especially after the PCP
        ///   server loses state.If the PCP client does not know the external
        ///   port, or does not have a preference, it MUST use 0.
        /// </value>
        public ushort SuggestedExternalPort { get; set; }

        /// <summary>
        ///   Suggested external IPv4 or IPv6 address.
        /// </summary>
        /// <value>
        ///   This is useful for refreshing a mapping, especially
        ///   after the PCP server loses state.If the PCP client does not know
        ///   the external address, or does not have a preference, it MUST use
        ///   the address-family-specific all-zeros address
        /// </value>
        public IPAddress SuggestedExternalAdddress { get; set; }

        /// <inheritdoc />
        public override void Write(NatWriter writer)
        {
            base.Write(writer);

            writer.WriteBytes(Nonce);
            writer.WriteByte(Protocol);
            writer.WriteByte(0); // reserved 24 bit (3 bytes)
            writer.WriteByte(0);
            writer.WriteByte(0);
            writer.WriteUInt16(InternalPort);
            writer.WriteUInt16(SuggestedExternalPort);
            writer.WriteIPv6Address(SuggestedExternalAdddress);
        }

        /// <inheritdoc />
        public override void Read(NatReader reader)
        {
            base.Read(reader);

            Nonce = reader.ReadBytes(NonceLength);
            Protocol = reader.ReadByte();
            reader.ReadByte(); // reserved 24 bit (3 bytes)
            reader.ReadByte();
            reader.ReadByte();
            InternalPort = reader.ReadUInt16();
            SuggestedExternalPort = reader.ReadUInt16();
            SuggestedExternalAdddress = reader.ReadIPv6Address();
        }
    }
}
