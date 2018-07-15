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
            var lease = new Lease
            {
                PublicAddress = IPAddress.Loopback,
                PublicPort = 1234,
                InternalPort = 1234,
                Lifetime = TimeSpan.FromSeconds(2)
            };
            var endpoint = new LeasedEndpoint(lease);

            await Task.Delay(TimeSpan.FromSeconds(2.5));
        }

        [TestMethod]
        public async Task Releasing()
        {
            var lease = new Lease
            {
                PublicAddress = IPAddress.Loopback,
                PublicPort = 1234,
                InternalPort = 1234,
                Lifetime = TimeSpan.FromSeconds(2)
            };
            var endpoint = new LeasedEndpoint(lease);
            await Task.Delay(TimeSpan.FromMilliseconds(40));

            await Task.Delay(TimeSpan.FromMilliseconds(40));
            endpoint.Dispose();
            await Task.Delay(TimeSpan.FromMilliseconds(400));
        }

    }
}
