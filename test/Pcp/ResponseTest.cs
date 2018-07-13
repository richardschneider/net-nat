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
    public class ResponseTest
    {
        [TestMethod]
        public void Defaults()
        {
            var msg = new Response();
            Assert.AreEqual(2, msg.Version);
            Assert.AreEqual(0, (int)msg.Opcode);
            Assert.AreEqual(0, msg.ResultCode);
            Assert.AreEqual(TimeSpan.Zero, msg.Lifetime);
            Assert.IsTrue(msg.IsResponse);
        }

    }
}
