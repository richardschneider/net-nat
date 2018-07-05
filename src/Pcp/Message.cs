using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat.Pcp
{
    /// <summary>
    ///   A Port Control Protocol message.
    /// </summary>
    public class Message : Pmp.Message
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <remarks>
        ///   Set <see cref="Version"/> to <see cref="Client.ProtocolVersion"/>.
        /// </remarks>
        public Message()
        {
            Version = Client.ProtocolVersion;
        }

    }
}
