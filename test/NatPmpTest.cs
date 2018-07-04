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
            var nat = new NatPmp(NatDiscovery.GetGateways().First());
            var q = await nat.IsAvailableAsync();
            Assert.IsTrue(q);
        }

    }
}
