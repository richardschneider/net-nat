using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat
{
    /// <summary>
    ///   A PMP operation code.
    /// </summary>
    public enum PmpOpcode : byte
    {
        /// <summary>
        ///   The external (public) address of the NAT.
        /// </summary>
        ExternalAddress = 0,

        /// <summary>
        ///   Map an UTP port.
        /// </summary>
        MapUtp = 1,

        /// <summary>
        ///   Map a TCP port.
        /// </summary>
        MapTcp = 2
    }
}
