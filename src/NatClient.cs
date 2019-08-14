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
    ///   Communicates with a NAT device.
    /// </summary>
    /// <remarks>
    ///   An abstract class that allows communication with a NAT device.
    /// </remarks>
    public abstract class NatClient
    {
        /// <summary>
        ///   The NAT port that receives requests.
        /// </summary>
        public const int RequestPort = 5351;

        UdpClient nat;

        /// <summary>
        ///   Creates a new instance of the <see cref="NatClient"/> class with the specified
        ///   IP Address of the NAT.
        /// </summary>
        /// <param name="address">
        ///   The IP address of the NAT server.
        /// </param>
        /// <param name="port">
        ///   The port of the NAT server.
        /// </param>
        public NatClient(IPAddress address, int port = RequestPort)
        {
            nat = new UdpClient(address.AddressFamily);
            nat.Connect(address, port);
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
        ///   The local endpoint.
        /// </summary>
        /// <value>
        ///   An <see cref="IPEndPoint"/>.
        /// </value>
        public IPEndPoint LocalEndPoint
        {
            get
            {
                return (IPEndPoint)nat.Client.LocalEndPoint;
            }
        }

        /// <summary>
        ///   The remote endpoint.
        /// </summary>
        /// <value>
        ///   An <see cref="IPEndPoint"/>.
        /// </value>
        public IPEndPoint RemoteEndPoint
        {
            get
            {
                return (IPEndPoint)nat.Client.RemoteEndPoint;
            }
        }

        /// <summary>
        ///   Determines if the NAT is online.
        /// </summary>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's result is
        ///   <b>true</b> if the NAT is online and speaks the correct protocol; otherwise, <b>false</b>.
        /// </returns>
        public abstract Task<bool> IsAvailableAsync();

        /// <summary>
        ///   The reason why the NAT is not availble.
        /// </summary>
        /// <value>
        ///   "" if the NAT is available, or a reason why it is not available. <b>null</b>
        ///   when <see cref="IsAvailableAsync"/> has not been called,
        /// </value>
        /// <remarks>
        ///   The property is only valid, after the <see cref="IsAvailableAsync"/> method is called.
        /// </remarks>
        public string UnavailableReason { get; protected set; }

        /// <summary>
        ///   Create an endpoint that can be connected to by devices outside of the NAT.
        /// </summary>
        /// <param name="protocol">
        ///   Either <see cref="ProtocolType.Tcp"/> or <see cref="ProtocolType.Udp"/>.
        /// </param>
        /// <param name="port">
        ///   The intenral port of the server.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's result is
        ///   a <see cref="Lease"/> that defines an endpoint that is connectable by 
        ///   devices not behind the NAT.
        /// </returns>
        public abstract Task<Lease> CreatePublicEndpointAsync(ProtocolType protocol, int port);

        /// <summary>
        ///   Renew the lease.
        /// </summary>
        /// <param name="lease">
        ///   The lease to renew.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's result is
        ///   a <see cref="Lease"/> that defines an endpoint that is connectable by 
        ///   devices not behind the NAT.
        /// </returns>
        /// <remarks>
        ///   The returned <see cref="Lease"/> may have different external address
        ///   and/or port than the original <paramref name="lease"/>.
        /// </remarks>
        public abstract Task<Lease> RenewPublicEndpointAsync(Lease lease);

        /// <summary>
        ///   Cancel the lease.
        /// </summary>
        /// <param name="lease">
        ///   The lease to break.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation.
        /// </returns>
        /// <remarks>
        ///   Tells that NAT that the public endpoint can be removed.
        /// </remarks>
        public abstract Task DeletePublicEndpointAsync(Lease lease);

        /// <summary>
        ///   TODO
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<byte[]> SendAndReceiveAsync(NatMessage request)
        {
            return SendAndReceiveAsync(request.ToByteArray());
        }

        /// <summary>
        ///   TODO
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<byte[]> SendAndReceiveAsync(byte[] request)
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
