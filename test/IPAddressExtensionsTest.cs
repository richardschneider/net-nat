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
    public class IPAddressExtensionsTest
    {
        readonly string[] privateAddress =
        {
            "127.0.0.1", "::1",
            "10.0.0.0", "10.255.255.255",
            "172.16.0.0", "172.31.255.255",
            "192.168.0.0", "192.168.255.255",
            "fd00:2016:22:dec::",
        };

        [TestMethod]
        public void PrivateAddresses()
        {
            foreach (var addr in privateAddress)
            {
                var ip = IPAddress.Parse(addr);
                Assert.IsTrue(ip.IsPrivate(), addr);
                Assert.IsFalse(ip.IsPublic(), addr);
            }
        }

        [TestMethod]
        public void MappedIPv4PrivateAddresses()
        {
            foreach (var addr in privateAddress)
            {
                var ip = IPAddress.Parse(addr);
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ip = IPAddress.Parse("::FFFF:" + addr);
                    Assert.IsTrue(ip.IsPrivate(), ip.ToString());
                    Assert.IsFalse(ip.IsPublic(), ip.ToString());
                }
            }
        }
    }
}
