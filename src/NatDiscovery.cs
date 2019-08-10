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
        ///   The network interfaces are queried for any gateway. A gateway device is 
        ///   typically also a NAT.
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

        /// <summary>
        ///   Gets the NATs.
        /// </summary>
        /// <returns>
        ///   A sequence of NAT clients that can be talked to, 
        /// </returns>
        /// <remarks>
        ///   Asks each <see cref="GetGateways">gateways</see> if it supports
        ///   PCP or PMP.  If true, then a <see cref="NatClient"/> is retuned.
        /// </remarks>
        public static IEnumerable<NatClient> GetNats()
        {
            foreach (var gateway in GetGateways())
            {
                NatClient nat = new Pcp.Client(gateway);
                if (nat.IsAvailableAsync().Result)
                {
                    yield return nat;
                    continue;
                }

                nat = new Pmp.Client(gateway);
                if (nat.IsAvailableAsync().Result)
                {
                    yield return nat;
                    continue;
                }
            }
        }

    }
}
