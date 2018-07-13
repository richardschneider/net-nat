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
            using (var server = new MockNat())
            {
                server.RequestReceived += (s, req) => server.udp.Send(new byte[] { 2, 128, 0, 0 }, 4, req.RemoteEndPoint);
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
    }
}
