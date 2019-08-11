using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Makaretu.Nat
{
    /// <summary>
    ///   A contract that rents a public endpoint from a NAT.
    /// </summary>
    public class Lease
    {
        /// <summary>
        ///   The NAT that owns the public endpoint.
        /// </summary>
        public NatClient Nat { get; set; }

        /// <summary>
        ///   A random value assigned to this lease.
        /// </summary>
        public byte[] Nonce { get; set; }

        /// <summary>
        ///   The public port assigned by the NAT.
        /// </summary>
        public IPAddress PublicAddress { get; set; }

        /// <summary>
        ///   The public IP address assigned by the NAT.
        /// </summary>
        public int PublicPort { get; set; }

        /// <summary>
        ///   The internal port for the public endpoint.
        /// </summary>
        public int InternalPort { get; set; }

        /// <summary>
        ///   The lifetime of the contract.
        /// </summary>
        /// <value>
        ///   Typically the number of seconds.
        /// </value>
        public TimeSpan Lifetime { get; set; }

    }
}
