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
            var endpoint = new LeasedEndpoint(null, IPAddress.Loopback, 1234, 1234, TimeSpan.FromSeconds(2));

            await Task.Delay(TimeSpan.FromSeconds(2.5));
        }

        [TestMethod]
        public async Task Releasing()
        {
            var endpoint = new LeasedEndpoint(null, IPAddress.Loopback, 1234, 1234, TimeSpan.FromSeconds(2));
            await Task.Delay(TimeSpan.FromMilliseconds(40));

            await Task.Delay(TimeSpan.FromMilliseconds(40));
            endpoint.Dispose();
            await Task.Delay(TimeSpan.FromMilliseconds(400));
        }

    }
}
