using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat
{
    /// <summary>
    ///   Port Mapping Protocol.
    /// </summary>
    /// <remarks>
    ///   Automates the process of creating Network Address Translation(NAT) port mappings.
    ///   Allows retrieving the external IPv4 address
    ///   of a NAT gateway, thus allowing a client to make its external IPv4
    ///   address and port known to peers that may wish to communicate with it.
    ///  <para>
    ///   NAT-PMP is  superseded by the IETF Standards Track RFC "Port Control Protocol
    ///   (PCP)", which builds on NAT-PMP and uses a compatible packet format,
    ///   but adds a number of significant enhancements.
    ///  </para>
    /// </remarks>
    /// <seealso href="https://tools.ietf.org/html/rfc6886">RFC 6886 - NAT Port Mapping Protocol</seealso>
    /// <seealso href="https://tools.ietf.org/html/rfc6887">RFC 6887 - NAT Port Control Protocol</seealso>
    public class NatPmp
    {
        /// <summary>
        ///   The NAT port that receives NAT-PMP requests.
        /// </summary>
        public const int RequestPort = 5351;

        /// <summary>
        ///   The version of the NAT-PMP.
        /// </summary>
        public const int ProtocolVersion = 0;

        UdpClient nat;

        /// <summary>
        ///   Creates a new instance of the <see cref="NatPmp"/> class with the specified
        ///   IP Address of the NAT.
        /// </summary>
        /// <param name="address">
        ///   The IP address of the NAT server.
        /// </param>
        public NatPmp(IPAddress address)
        {
            nat = new UdpClient(address.AddressFamily);
            nat.Connect(address, RequestPort);
        }

        /// <summary>
        ///   The time to wait for a response from NAT.
        /// </summary>
        /// <value>
        ///   Defaults to 250ms.
        /// </value>
        public TimeSpan InitialTimeout { get; set; } = TimeSpan.FromMilliseconds(250);

        /// <summary>
        ///   Number of times to retry sending request.
        /// </summary>
        /// <value>
        ///   Defaults to 4.
        /// </value>
        public int MaxRetries { get; set; } = 4;

        /// <summary>
        ///   Determines if the NAT is online.
        /// </summary>
        /// <returns>
        ///   <b>true</b> if the NAT is online and speaks NAT-PMP; otherwise, <b>fale</b>.
        /// </returns>
        public async Task<bool> IsAvailableAsync()
        {
            var hello = new byte[2];
            try
            {
                var res = await SendAsync(hello);
                if (res[0] != ProtocolVersion)
                    return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        async Task<byte[]> SendAsync(byte[] request)
        {
            int timeout = (int) InitialTimeout.TotalMilliseconds;
            int retries = 0;
            do
            {
                await nat.SendAsync(request, request.Length);

                var task = nat.ReceiveAsync();
                if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
                {
                    // task completed within timeout
                    return task.Result.Buffer;
                }

                // Timeout.
                timeout *= 2;
            } while (retries++ < MaxRetries);

            throw new TimeoutException();
        }
    }
}
