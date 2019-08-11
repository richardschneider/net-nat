using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat.Pcp
{
    [TestClass]
    public class ClientTest
    {
        [TestMethod]
        public async Task IsAvailable()
        {
            var response = new Response().ToByteArray();
            using (var server = new MockNat())
            {
                server.RequestReceived += (s, req) => server.udp.Send(response, response.Length, req.RemoteEndPoint);
                var nat = new Client(server.Address);
                var q = await nat.IsAvailableAsync();
                Assert.IsTrue(q);
            }
        }

        [TestMethod]
        public async Task IsAvailble_NotNat()
        {
            var nat = new Client(IPAddress.Parse("127.0.0.1"));
            var q = await nat.IsAvailableAsync();
            Assert.IsFalse(q);
        }

        [TestMethod]
        public async Task Unresponsive_Nat()
        {
            using (var server = new MockNat())
            {
                var nat = new Client(server.Address) { MaxRetries = 1 };
                var q = await nat.IsAvailableAsync();
                Assert.IsFalse(q);
            }
        }

        [TestMethod]
        public async Task CreateDeleteEndpoint()
        {
            var ttl = TimeSpan.FromSeconds(10);
            using (var server = new MockNat())
            {
                server.RequestReceived += (s, req) =>
                {
                    var map = Message.Create<MapRequest>(req.Buffer);
                    var response = new MapResponse
                    {
                        AssignedExternalAdddress = IPAddress.Loopback,
                        AssignedExternalPort = 1234,
                        EpochTime = TimeSpan.FromSeconds(1),
                        Lifetime = ttl,
                        Opcode = Opcode.Map,
                        Nonce = map.Nonce,
                        InternalPort = map.InternalPort,
                        Protocol = map.Protocol
                    }.ToByteArray();
                    server.udp.Send(response, response.Length, req.RemoteEndPoint);
                };
                var nat = new Client(server.Address);

                var lease = await nat.CreatePublicEndpointAsync(ProtocolType.Udp, 4321);
                Assert.AreEqual(IPAddress.Loopback, lease.PublicAddress);
                Assert.AreEqual(4321, lease.InternalPort);
                Assert.AreEqual(1234, lease.PublicPort);
                Assert.AreEqual(ttl, lease.Lifetime);
                Assert.AreEqual(ProtocolType.Udp, lease.Protocol);

                await nat.DeletePublicEndpointAsync(lease);
            }
        }
    }
}
