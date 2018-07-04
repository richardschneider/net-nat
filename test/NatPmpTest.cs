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
    public class NatPmpTest
    {
        [TestMethod]
        public async Task IsAvailable()
        {
            using (var server = new MockNat())
            {
                server.RequestReceived += (s, req) => server.udp.Send(new byte[2], 2, req.RemoteEndPoint); 
                var nat = new NatPmp(server.Address);
                var q = await nat.IsAvailableAsync();
                Assert.IsTrue(q);
            }
        }

        [TestMethod]
        public async Task Unresponsive_Nat()
        {
            using (var server = new MockNat())
            {
                var nat = new NatPmp(server.Address) { MaxRetries = 1 };
                var q = await nat.IsAvailableAsync();
                Assert.IsFalse(q);
            }
        }

    }
}
