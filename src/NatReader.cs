using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat
{
    /// <summary>
    ///   Reads NAT data items from a stream.
    /// </summary>
    /// <remarks>
    ///   <b>NatReader</b> is used to read data items from a NAT-PCP or NAT-PMP stream.
    /// </remarks>
    public class NatReader
    {
        Stream stream;

        /// <summary>
        ///   The position within the stream.
        /// </summary>
        /// <value>
        ///   The current position in the stream.
        /// </value>
        public int Position
        {
            get { return (int) stream.Position; }
            set { stream.Position = value; }
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="NatReader"/> on the
        ///   specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">
        ///   The destination for data items.
        /// </param>
        public NatReader(Stream stream)
        {
            this.stream = stream;
        }

        /// <summary>
        ///   Read the specified number of bytes.
        /// </summary>
        /// <param name="length">
        ///   The number of bytes to read.
        /// </param>
        /// <returns>
        ///   The next <paramref name="length"/> bytes in the stream.
        /// </returns>
        /// <exception cref="EndOfStreamException">
        ///   When no more data is available.
        /// </exception>
        public byte[] ReadBytes(int length)
        {
            var buffer = new byte[length];
            for (var offset = 0; length > 0;)
            {
                var n = stream.Read(buffer, offset, length);
                if (n == 0)
                    throw new EndOfStreamException();
                offset += n;
                length -= n;
            }

            return buffer;
        }

        /// <summary>
        ///   Read a byte.
        /// </summary>
        /// <returns>
        ///   The next byte in the stream.
        /// </returns>
        /// <exception cref="EndOfStreamException">
        ///   When no more data is available.
        /// </exception>
        public byte ReadByte()
        {
            var value = stream.ReadByte();
            if (value < 0)
                throw new EndOfStreamException();
            return (byte)value;
        }

        /// <summary>
        ///   Read an unsigned short.
        /// </summary>
        /// <returns>
        ///   The two byte little-endian value as an unsigned short.
        /// </returns>
        /// <exception cref="EndOfStreamException">
        ///   When no more data is available.
        /// </exception>
        public ushort ReadUInt16()
        {
            int value = ReadByte();
            value = value << 8 | ReadByte();
            return (ushort)value;
        }

        /// <summary>
        ///   Read an unsigned int.
        /// </summary>
        /// <returns>
        ///   The four byte little-endian value as an unsigned int.
        /// </returns>
        /// <exception cref="EndOfStreamException">
        ///   When no more data is available.
        /// </exception>
        public uint ReadUInt32()
        {
            int value = ReadByte();
            value = value << 8 | ReadByte();
            value = value << 8 | ReadByte();
            value = value << 8 | ReadByte();
            return (uint)value;
        }

        /// <summary>
        ///   Read a time span (interval)
        /// </summary>
        /// <returns>
        ///   A <see cref="TimeSpan"/> with second resolution.
        /// </returns>
        /// <exception cref="EndOfStreamException">
        ///   When no more data is available.
        /// </exception>
        /// <remarks>
        ///   The interval is represented as the number of seconds in four bytes.
        /// </remarks>
        public TimeSpan ReadTimeSpan()
        {
            return TimeSpan.FromSeconds(ReadUInt32());
        }

        /// <summary>
        ///   Read an IPv4 address.
        /// </summary>
        public IPAddress ReadIPv4Address()
        {
            var address = ReadBytes(4);
            return new IPAddress(address);
        }

        /// <summary>
        ///   Read an IPv6 address.
        /// </summary>
        public IPAddress ReadIPv6Address()
        {
            var address = ReadBytes(16);
            return new IPAddress(address);
        }

    }
}
