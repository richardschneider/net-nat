using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Makaretu.Nat
{
    /// <summary>
    ///   Writes NAT data items to stream.
    /// </summary>
    /// <remarks>
    ///   <b>NatWriter</b> is used to write data items to a NAT-PCP or NAT-PMP stream.
    /// </remarks>
    public class NatWriter
    {
        Stream stream;

        /// <summary>
        ///   The writer relative position within the stream.
        /// </summary>
        public int Position;

        /// <summary>
        ///   Creates a new instance of the <see cref="NatWriter"/> on the
        ///   specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">
        ///   The destination for data items.
        /// </param>
        public NatWriter(Stream stream)
        {
            this.stream = stream;
        }

        /// <summary>
        ///   Write a sequence of bytes.
        /// </summary>
        /// <param name="bytes"></param>
        public void WriteBytes(byte[] bytes)
        {
            if (bytes != null)
            {
                stream.Write(bytes, 0, bytes.Length);
                Position += bytes.Length;
            }
        }

        /// <summary>
        ///   Write a byte.
        /// </summary>
        public void WriteByte(byte value)
        {
            stream.WriteByte(value);
            ++Position;
        }

        /// <summary>
        ///   Write an unsigned short.
        /// </summary>
        public void WriteUInt16(ushort value)
        {
            stream.WriteByte((byte)(value >> 8));
            stream.WriteByte((byte)value);
            Position += 2;
        }

        /// <summary>
        ///   Write an unsigned int.
        /// </summary>
        public void WriteUInt32(uint value)
        {
            stream.WriteByte((byte)(value >> 24));
            stream.WriteByte((byte)(value >> 16));
            stream.WriteByte((byte)(value >> 8));
            stream.WriteByte((byte)value);
            Position += 4;
        }

        /// <summary>
        ///   Write a time span.
        /// </summary>
        /// <remarks>
        ///   Represented as 32-bit unsigned int (in seconds).
        /// </remarks>
        public void WriteTimeSpan(TimeSpan value)
        {
            WriteUInt32((uint)value.TotalSeconds);
        }

        /// <summary>
        ///   Write an IPv4 address.
        /// </summary>
        /// <param name="value"></param>
        public void WriteIPv4Address(IPAddress value)
        {
            // TODO: Throw if IPv6
            WriteBytes(value.GetAddressBytes());
        }

        /// <summary>
        ///   Write an IPv6 address.
        /// </summary>
        /// <param name="value"></param>
        public void WriteIPv6Address(IPAddress value)
        {
            // Map IPv4 to IPv6
            if (value.AddressFamily == AddressFamily.InterNetwork)
            {
                value = value.MapToIPv6();
            }
            WriteBytes(value.GetAddressBytes());
        }
    }
}
