using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat
{
    /// <summary>
    ///   NAT extensions to the <see cref="IPAddress"/> class.
    /// </summary>
    /// <remarks>
    ///   <see cref="IsPrivate(IPAddress)"/> and <see cref="IsPublic(IPAddress)"/> determines if an IP address is reachable
    ///   from the public internet; see <see href="https://tools.ietf.org/html/rfc1918">RFC 1918</see> for more details.
    /// </remarks>
    public static class IPAddressExtensions
    {
        readonly static IPNetwork[] privateNetworks = new IPNetwork[]
        {
            IPNetwork.Parse("192.168.0.0/16"),
            IPNetwork.Parse("10.0.0.0/8"),
            IPNetwork.Parse("172.16.0.0/12"),
            IPNetwork.Parse("fc00::/7"),
        };

        /// <summary>
        ///   Determines if the IP address belongs to a private internet.
        /// </summary>
        /// <param name="address">
        ///   The IP address to test.
        /// </param>
        /// <returns>
        ///   <b>true</b> if the <paramref name="address"/> is private; otherwise, <b>false</b>.
        /// </returns>
        /// <remarks>
        ///   <see href="https://tools.ietf.org/html/rfc1918">RFC 1918</see> defines the following as
        ///   private addresses (in CIDR notation)
        ///   <list type="bullet">
        ///   <item><description>10/8</description></item>
        ///   <item><description>172.16/12</description></item>
        ///   <item><description>192.168/16</description></item>
        ///   </list>
        ///   <para>
        ///   Loop back addresses are considered private.
        ///   </para>
        ///   <para>
        ///   If the <paramref name="address"/> <see cref="IPAddress.IsIPv4MappedToIPv6"/>,
        ///   then the IPv4 address is tested.
        ///   </para>
        ///   IPv6 <see href="https://en.wikipedia.org/wiki/Unique_local_address">ULA</see>
        ///   addresses are always private.
        /// </remarks>
        public static bool IsPrivate(this IPAddress address)
        {
            if (address.IsIPv4MappedToIPv6)
            {
                address = address.MapToIPv4();
            }

            if (IPAddress.IsLoopback(address))
                return true;


            return privateNetworks.Any(n => n.Contains(address));
        }

        /// <summary>
        ///   Determines if the IP address does not belong to a private internet.
        /// </summary>
        /// <param name="address">
        ///   The IP address to test.
        /// </param>
        /// <returns>
        ///   <b>true</b> if the <paramref name="address"/> is public; otherwise, <b>false</b>.
        /// </returns>
        /// <remarks>
        ///   This is the inverse of <see cref="IsPrivate(IPAddress)"/>.
        /// </remarks>
        public static bool IsPublic(this IPAddress address)
        {
            return !address.IsPrivate();
        }
    }
}
