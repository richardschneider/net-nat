﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
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
    public class Client : NatClient
    {
        /// <summary>
        ///   The version of the NAT-PCP.
        /// </summary>
        public const int ProtocolVersion = 2;

        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

        /// <summary>
        ///   Creates a new instance of the <see cref="Client"/> class with the specified
        ///   IP Address of the NAT.
        /// </summary>
        /// <param name="address">
        ///   The IP address of the NAT server.
        /// </param>
        public Client(IPAddress address) : base(address)
        {
        }

        /// <inheritdoc />
        public override async Task<bool> IsAvailableAsync()
        {
            var hello = new AnnounceRequest
            {
                ClientAddress = LocalEndPoint.Address
            };
            try
            {
                var res = await SendAndReceiveAsync(hello);
                var response = Message.Create<Response>(res);
                switch (response.ResultCode)
                {
                    case 0:
                        if (response.Version == ProtocolVersion)
                        {
                            UnavailableReason = "";
                            return true;
                        }

                        UnavailableReason = "Protocol version not support.";
                        return false;
                    case 2:
                        UnavailableReason = "Unathorised";
                        return false;
                    default:
                        UnavailableReason = $"Error code {response.ResultCode}.";
                        return false;
                }
            }
            catch (TimeoutException)
            {
                UnavailableReason = "No response received.";
                return false;
            }
            catch (Exception e)
            {
                UnavailableReason = $"Unexpected exception '{e.GetType().FullName}'. {e.Message}.";
                return false;
            }
        }

        /// <inheritdoc />
        public override async Task<Lease> CreatePublicEndpointAsync(ProtocolType protocol, int port)
        {
            var map = new MapRequest
            {
                ClientAddress = LocalEndPoint.Address,
                InternalPort = (ushort)port,
                Nonce = GenerateNonce(),
                Protocol = protocol,
                RequestedLifetime = TimeSpan.FromHours(1), // TODO
                SuggestedExternalAdddress = LocalEndPoint.AddressFamily == AddressFamily.InterNetwork
                    ? IPAddress.Any
                    : IPAddress.IPv6Any,
                SuggestedExternalPort = (ushort)port
            };
            var res = await SendAndReceiveAsync(map);
            var response = Message.Create<MapResponse>(res);
            response.EnsureSuccess();
            var address = response.AssignedExternalAdddress;
            if (address.IsIPv4MappedToIPv6)
            {
                address = address.MapToIPv4();
            }
            var lease = new Lease
            {
                Nat = this,
                Nonce = response.Nonce,
                Protocol = response.Protocol,
                InternalPort = response.InternalPort,
                Lifetime = response.Lifetime,
                PublicAddress = address,
                PublicPort = response.AssignedExternalPort
            };
            return lease;
        }

        /// <inheritdoc />
        public override async Task<Lease> RenewPublicEndpointAsync(Lease lease)
        {
            Console.WriteLine($"Need to renew {lease.PublicAddress}:{lease.PublicPort}");
            var map = new MapRequest
            {
                ClientAddress = LocalEndPoint.Address,
                InternalPort = (ushort)lease.InternalPort,
                Nonce = lease.Nonce,
                Protocol = lease.Protocol,
                RequestedLifetime = TimeSpan.FromHours(1), // TODO
                SuggestedExternalAdddress = lease.PublicAddress,
                SuggestedExternalPort = (ushort) lease.PublicPort
            };
            var res = await SendAndReceiveAsync(map);
            var response = Message.Create<MapResponse>(res);
            response.EnsureSuccess();
            var address = response.AssignedExternalAdddress;
            if (address.IsIPv4MappedToIPv6)
            {
                address = address.MapToIPv4();
            }
            var renewal = new Lease
            {
                Nat = this,
                Nonce = response.Nonce,
                Protocol = response.Protocol,
                InternalPort = response.InternalPort,
                Lifetime = response.Lifetime,
                PublicAddress = address,
                PublicPort = response.AssignedExternalPort
            };
            return renewal;
        }

        /// <inheritdoc />
        public override async Task DeletePublicEndpointAsync(Lease lease)
        {
            var map = new MapRequest
            {
                ClientAddress = LocalEndPoint.Address,
                InternalPort = (ushort)lease.InternalPort,
                Nonce = GenerateNonce(),
                Protocol = lease.Protocol,
                RequestedLifetime = TimeSpan.Zero, // indicates delete
                SuggestedExternalAdddress = lease.PublicAddress,
                SuggestedExternalPort = (ushort)lease.PublicPort
            };
            var res = await SendAndReceiveAsync(map);
            var response = Message.Create<MapResponse>(res);
            response.EnsureSuccess();
        }

        byte[] GenerateNonce()
        {
            var nonce = new byte[Message.NonceLength];
            rng.GetBytes(nonce);
            return nonce;
        }

    }
}
