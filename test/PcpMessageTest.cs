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
    public class PcpMessageTest
    {
        [TestMethod]
        public void Default()
        {
            var msg = new PcpMessage();
            Assert.AreEqual(2, msg.Version);
            Assert.AreEqual(0, msg.Opcode);
        }

        [TestMethod]
        public void Length()
        {
            var msg = new PcpMessage();
            Assert.AreEqual(2, msg.Length());
        }

    }
}
