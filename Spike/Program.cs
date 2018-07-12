using Makaretu.Nat;
using System;

namespace Spike
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("NAT Spike!");

            Console.WriteLine();
            Console.WriteLine("Gateways");
            foreach (var g in NatDiscovery.GetGateways())
            {
                bool q;
                Console.WriteLine($"  {g}");

                var pcp = new Makaretu.Nat.Pcp.Client(g);
                q = pcp.IsAvailableAsync().Result;
                Console.WriteLine($"    supports NAT-PCP {q}");

                var pmp = new Makaretu.Nat.Pmp.Client(g);
                q = pmp.IsAvailableAsync().Result;
                Console.WriteLine($"    supports NAT-PMP {q}");
            }

        }
    }
}
