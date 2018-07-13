using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Makaretu.Nat
{
    /// <summary>
    ///   Base class for all NAT related messages.
    /// </summary>
    /// <remarks>
    ///   Provides serialisation services, Read and Write.
    /// </remarks>
    public abstract class NatMessage : ICloneable
    {
        /// <summary>
        ///   Length in bytes of the object when serialised.
        /// </summary>
        /// <returns>
        ///   Numbers of bytes when serialised.
        /// </returns>
        public int Length()
        {
            var writer = new NatWriter(Stream.Null);
            Write(writer);

            return writer.Position;
        }

        /// <summary>
        ///   Makes a deep copy of the object.
        /// </summary>
        /// <returns>
        ///   A deep copy of the NAT message.
        /// </returns>
        /// <remarks>
        ///   Uses serialisation to make a copy.
        /// </remarks>
        public object Clone()
        {
            throw new NotImplementedException();
#if false
            using (var ms = new MemoryStream())
            {
                Write(ms);
                ms.Position = 0;
                return Read(ms);
            }
#endif
        }

        /// <summary>
        ///   Makes a deep copy of the object.
        /// </summary>
        /// <typeparam name="T">
        ///   Some type derived from <see cref="NatMessage"/>.
        /// </typeparam>
        /// <returns>
        ///   A deep copy of the dns object.
        /// </returns>
        /// <remarks>
        ///   Use serialisation to make a copy.
        /// </remarks>
        public T Clone<T>() where T : NatMessage
        {
            return (T)Clone();
        }

#if false
        /// <summary>
        ///   Reads the message from a byte array.
        /// </summary>
        /// <param name="buffer">
        ///   The source for the NAT message.
        /// </param>
        public static NatMessage Read(byte[] buffer)
        {
            return Read(buffer, 0, buffer.Length);
        }

        /// <summary>
        ///   Reads the message from a byte array.
        /// </summary>
        /// <param name="buffer">
        ///   The source for the NAT message.
        /// </param>
        /// <param name="offset">
        ///   The offset into the <paramref name="buffer"/>.
        /// </param>
        /// <param name="count">
        ///   The number of bytes in the <paramref name="buffer"/>.
        /// </param>
        public static NatMessage Read(byte[] buffer, int offset, int count)
        {
            using (var ms = new MemoryStream(buffer, offset, count, false))
            {
                return Read(new NatReader(ms));
            }
        }

        /// <summary>
        ///   Reads the DNS object from a stream.
        /// </summary>
        /// <param name="stream">
        ///   The source for the DNS object.
        /// </param>
        public IDnsSerialiser Read(Stream stream)
        {
            return Read(new DnsReader(stream));
        }

        /// <inheritdoc />
        public abstract IDnsSerialiser Read(DnsReader reader);
#endif
        /// <summary>
        ///   Writes the NAT message to a byte array.
        /// </summary>
        /// <returns>
        ///   A byte array containing the binary representaton of the NAT message.
        /// </returns>
        public byte[] ToByteArray()
        {
            using (var ms = new MemoryStream())
            {
                Write(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        ///   Writes the NAT message to a stream.
        /// </summary>
        /// <param name="stream">
        ///   The destination for the NAT message.
        /// </param>
        public void Write(Stream stream)
        {
            Write(new NatWriter(stream));
        }

        /// <summary>
        ///   Writes the NAT message.
        /// </summary>
        /// <param name="writer">
        ///   The destination for the NAT message.
        /// </param>
        /// <remarks>
        ///   Derived classes must override this.
        /// </remarks>
        public abstract void Write(NatWriter writer);

        /// <summary>
        ///   Reads the NAT message.
        /// </summary>
        /// <param name="reader">
        ///   The source for the NAT message.
        /// </param>
        /// <remarks>
        ///   Derived classes must override this.
        /// </remarks>
        public abstract void Read(NatReader reader);
    }
}
