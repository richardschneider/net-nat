using Makaretu.Nat.Pcp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat
{
    [TestClass]
    public class LeasedEndpointTest
    {
        [TestMethod]
        public async Task Expires()
        {
            var lease = new Lease
            {
                PublicAddress = IPAddress.Loopback,
                PublicPort = 1234,
                InternalPort = 1234,
                Lifetime = TimeSpan.FromSeconds(1)
            };

            using (var endpoint = new LeasedEndpoint(lease))
            {
                await Task.Delay(TimeSpan.FromSeconds(2.5));
            }

        }

        [TestMethod]
        public async Task Releasing()
        {
            var lease = new Lease
            {
                PublicAddress = IPAddress.Loopback,
                PublicPort = 1234,
                InternalPort = 1234,
                Lifetime = TimeSpan.FromSeconds(2)
            };
            var endpoint = new LeasedEndpoint(lease);
            await Task.Delay(TimeSpan.FromMilliseconds(40));

            await Task.Delay(TimeSpan.FromMilliseconds(40));
            endpoint.Dispose();
            await Task.Delay(TimeSpan.FromMilliseconds(400));
        }

        [TestMethod]
        public async Task SendsRenewal()
        {
            int nRequests = 0;
            using (var server = new MockNat())
            {
                server.RequestReceived += (s, req) => ++nRequests;
                var nat = new Pcp.Client(server.Address);
                var lease = new Lease
                {
                    Nat = nat,
                    Nonce = new byte[12],
                    PublicAddress = IPAddress.Loopback,
                    PublicPort = 1234,
                    InternalPort = 1234,
                    Lifetime = TimeSpan.FromSeconds(1)
                };
                using (var endpoint = new LeasedEndpoint(lease))
                {
                    await Task.Delay(lease.Lifetime);
                    Assert.AreNotEqual(0, nRequests);
                }
            }
        }

        [TestMethod]
        public async Task RaisesChanged_OnPortChange()
        {
            using (var server = new MockNat())
            {
                server.RequestReceived += (s, req) =>
                {
                    var map = Message.Create<MapRequest>(req.Buffer);
                    var response = new MapResponse
                    {
                        AssignedExternalAdddress = map.SuggestedExternalAdddress,
                        AssignedExternalPort = 4321,
                        EpochTime = TimeSpan.FromSeconds(1),
                        Lifetime = TimeSpan.FromSeconds(1),
                        Opcode = Opcode.Map,
                        Nonce = map.Nonce,
                        InternalPort = map.InternalPort,
                        Protocol = map.Protocol
                    }.ToByteArray();
                    server.udp.Send(response, response.Length, req.RemoteEndPoint);
                };
                var nat = new Pcp.Client(server.Address);
                var lease = new Lease
                {
                    Nat = nat,
                    Nonce = new byte[12],
                    PublicAddress = IPAddress.Loopback,
                    PublicPort = 1234,
                    InternalPort = 1234,
                    Lifetime = TimeSpan.FromSeconds(1)
                };
                using (var endpoint = new LeasedEndpoint(lease))
                {
                    int nChanges = 0;
                    endpoint.Changed += (s, e) => ++nChanges;
                    await Task.Delay(lease.Lifetime);
                    Assert.AreNotEqual(0, nChanges);
                }
            }
        }

        [TestMethod]
        public async Task RaisesChanged_OnAddressChange()
        {
            using (var server = new MockNat())
            {
                server.RequestReceived += (s, req) =>
                {
                    var map = Message.Create<MapRequest>(req.Buffer);
                    var response = new MapResponse
                    {
                        AssignedExternalAdddress = IPAddress.Broadcast,
                        AssignedExternalPort = map.SuggestedExternalPort,
                        EpochTime = TimeSpan.FromSeconds(1),
                        Lifetime = TimeSpan.FromSeconds(1),
                        Opcode = Opcode.Map,
                        Nonce = map.Nonce,
                        InternalPort = map.InternalPort,
                        Protocol = map.Protocol
                    }.ToByteArray();
                    server.udp.Send(response, response.Length, req.RemoteEndPoint);
                };
                var nat = new Pcp.Client(server.Address);
                var lease = new Lease
                {
                    Nat = nat,
                    Nonce = new byte[12],
                    PublicAddress = IPAddress.Loopback,
                    PublicPort = 1234,
                    InternalPort = 1234,
                    Lifetime = TimeSpan.FromSeconds(1)
                };
                using (var endpoint = new LeasedEndpoint(lease))
                {
                    int nChanges = 0;
                    endpoint.Changed += (s, e) => ++nChanges;
                    await Task.Delay(lease.Lifetime);
                    Assert.AreNotEqual(0, nChanges);
                }
            }
        }
    }
}
