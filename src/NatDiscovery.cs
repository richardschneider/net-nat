using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat
{
    /// <summary>
    ///   Discoveres the possible NATs.
    /// </summary>
    /// <remarks>
    ///   A gateway to the internet can be a NAT.
    /// </remarks>
    public static class NatDiscovery
    {
        /// <summary>
        ///   Get the addresses of gateway(s) to the internet.
        /// </summary>
        /// <returns>
        ///   A sequence of gateway IP addresses. 
        /// </returns>
        /// <remarks>
        ///   A gateway device is typically also a NAT.
        /// </remarks>
        public static IEnumerable<IPAddress> GetGateways()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up)
                .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(n => n.GetIPProperties()?.GatewayAddresses)
                .Select(g => g?.Address)
                .Where(a => a != null)
                .Where(a => a.AddressFamily == AddressFamily.InterNetwork 
                    || a.AddressFamily == AddressFamily.InterNetworkV6)
                .Distinct()
                ;
        }

        /// <summary>
        ///   Get the IP addresses of the local machine.
        /// </summary>
        /// <returns>
        ///   A sequence of IP addresses of the local machine.
        /// </returns>
        public static IEnumerable<IPAddress> GetIPAddresses()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up)
                .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(nic => nic.GetIPProperties().UnicastAddresses)
                .Select(u => u.Address);
        }

    }
}
