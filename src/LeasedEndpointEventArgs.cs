using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat
{
    /// <summary>
    ///   The leases related to a <see cref="LeasedEndpoint"/>.
    /// </summary>
    /// <remarks>
    ///   Is used by <see cref="LeasedEndpoint.Changed"/> to indicate
    ///   that the public address and/or port has changed.
    /// </remarks>
    public class LeasedEndpointEventArgs : EventArgs
    {
        /// <summary>
        ///   The previous lease.
        /// </summary>
        public Lease Previous { get; set; }

        /// <summary>
        ///   The current lease.
        /// </summary>
        public Lease Current { get; set; }
    }
}
