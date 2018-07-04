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
    public class NatPcpTest
    {
        [TestMethod]
        public async Task IsAvailable()
        {
            var nat = new NatPcp(NatDiscovery.GetGateways().First());
            var q = await nat.IsAvailableAsync();
            Assert.IsTrue(q);
        }

    }
}
