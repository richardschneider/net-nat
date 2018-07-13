using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat.Pcp
{
    /// <summary>
    ///   A Port Control Protocol response message.
    /// </summary>
    public class Response : Message
    {
        static byte[] reserved2 = new byte[96 / 8];

        public Response()
        {
            IsResponse = true;
        }

        public byte ResultCode { get; set; }

        /// <summary>
        ///   The lifetime of the operation.
        /// </summary>
        /// <value>
        ///   Resolution in seconds.
        /// </value>
        public TimeSpan Lifetime { get; set; }

        /// <inheritdoc />
        public override void Write(NatWriter writer)
        {
            base.Write(writer);

            writer.WriteByte(0); // reserved
            writer.WriteByte(ResultCode);
            writer.WriteTimeSpan(Lifetime);
            writer.WriteBytes(reserved2);
        }

        /// <inheritdoc />
        public override void Read(NatReader reader)
        {
            base.Read(reader);

            reader.ReadByte(); // reserved
            ResultCode = reader.ReadByte();
            Lifetime = reader.ReadTimeSpan();
            reader.ReadBytes(reserved2.Length);
        }

    }
}
