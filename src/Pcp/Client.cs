using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat.Pcp
{
    /// <summary>
    ///   Communicates with a NAT that speaks the Port Control Protocol.
    /// </summary>
    /// <remarks>
    ///  PCP allows applications to create mappings from an external IP
    ///  address, protocol, and port to an internal IP address, protocol, and
    ///  port. These mappings are required for successful inbound
    ///  communications destined to machines located behind a NAT or a
    ///  firewall.
    /// </remarks>
    /// <seealso href="https://tools.ietf.org/html/rfc6887">RFC 6887 - NAT Port Control Protocol</seealso>
    public class Client
    {
        /// <summary>
        ///   The NAT port that receives NAT-PCP requests.
        /// </summary>
        public const int RequestPort = Pmp.Client.RequestPort;

        /// <summary>
        ///   The version of the NAT-PCP.
        /// </summary>
        public const int ProtocolVersion = 2;

        UdpClient nat;

        /// <summary>
        ///   Creates a new instance of the <see cref="Client"/> class with the specified
        ///   IP Address of the NAT.
        /// </summary>
        /// <param name="address">
        ///   The IP address of the NAT server.
        /// </param>
        public Client(IPAddress address)
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
        ///   <b>true</b> if the NAT is online and speaks NAT-PCP; otherwise, <b>fale</b>.
        /// </returns>
        public async Task<bool> IsAvailableAsync()
        {
            var hello = new byte[8 + 16];
            hello[0] = ProtocolVersion;
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
            int timeout = (int)InitialTimeout.TotalMilliseconds;
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
