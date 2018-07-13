using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat.Pcp
{
    /// <summary>
    ///   Requests information about the NAT server.
    /// </summary>
    public class AnnounceRequest : Request
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="AnnounceRequest"/> class.
        /// </summary>
        public AnnounceRequest()
        {
            Opcode = Opcode.Announce;
        }
    }
}
