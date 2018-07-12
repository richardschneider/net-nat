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
    public class NatDiscoveryTest
    {
        [TestMethod]
        public void Gateways()
        {
            var gateways = NatDiscovery.GetGateways();
            Assert.AreNotEqual(0, gateways.Count(), "no gateway found");
            foreach (var g in gateways)
            {
                Console.WriteLine($"Gateway at {g}");
            }
        }

        [TestMethod]
        public void IPAddresses()
        {
            var addresses = NatDiscovery.GetIPAddresses();
            Assert.AreNotEqual(0, addresses.Count(), "no IP address found");
            foreach (var a in addresses)
            {
                Console.WriteLine($"{a} public={a.IsPublic()}");
            }
        }

    }
}
