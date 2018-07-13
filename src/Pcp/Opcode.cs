using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat.Pcp
{
    /// <summary>
    ///   A port controlling operation code.
    /// </summary>
    public enum Opcode : byte
    {
        /// <summary>
        ///   The external (public) address of the NAT.
        /// </summary>
        Announce = 0,

        /// <summary>
        ///   Map an incoming port.
        /// </summary>
        Map = 1,

        /// <summary>
        ///   Map an outgoing port
        /// </summary>
        Peer = 2
    }
}
