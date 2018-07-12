using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat.Pmp
{
    /// <summary>
    ///   A port mapping operation code.
    /// </summary>
    public enum Opcode : byte
    {
        /// <summary>
        ///   The external (public) address of the NAT.
        /// </summary>
        ExternalAddress = 0,

        /// <summary>
        ///   Map an UDP port.
        /// </summary>
        MapUdp = 1,

        /// <summary>
        ///   Map a TCP port.
        /// </summary>
        MapTcp = 2
    }
}
