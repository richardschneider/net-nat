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

        /// <summary>
        ///   Creates a new instance of the <see cref="Response"/> class.
        /// </summary>
        public Response()
        {
            IsResponse = true;
        }

        /// <summary>
        ///   Success or failure indicator.
        /// </summary>
        /// <value>
        ///   0 for success; otherwise, failure.
        /// </value>
        public byte ResultCode { get; set; }

        /// <summary>
        ///   The lifetime of the operation.
        /// </summary>
        /// <value>
        ///   Resolution in seconds.
        /// </value>
        public TimeSpan Lifetime { get; set; }

        /// <summary>
        ///   The current time of the NAT server.
        /// </summary>
        /// <value>
        ///   Typically, the number of seconds since the last reset.
        /// </value>
        public TimeSpan EpochTime { get; set; }

        /// <inheritdoc />
        public override void Write(NatWriter writer)
        {
            base.Write(writer);

            writer.WriteByte(0); // reserved
            writer.WriteByte(ResultCode);
            writer.WriteTimeSpan(Lifetime);
            writer.WriteTimeSpan(EpochTime);
            writer.WriteBytes(reserved2);
        }

        /// <inheritdoc />
        public override void Read(NatReader reader)
        {
            base.Read(reader);

            reader.ReadByte(); // reserved
            ResultCode = reader.ReadByte();
            Lifetime = reader.ReadTimeSpan();
            EpochTime = reader.ReadTimeSpan();
            reader.ReadBytes(reserved2.Length);
        }
        
        /// <summary>
        ///   Throws an exception if the result code indicates failure.
        /// </summary>
        /// <seealso cref="ResultCode"/>
        public void EnsureSuccess()
        {
            if (ResultCode != 0)
            {
                throw new Exception($"NAT-PCP failure ({ResultCode}).");
            }
        }
    }
}
