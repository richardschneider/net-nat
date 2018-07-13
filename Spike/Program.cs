using Makaretu.Nat;
using System;
using System.Net;

namespace Spike
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("NAT Spike!");

            Console.WriteLine();
            Console.WriteLine("Unicast addresses");
            foreach (var a in NatDiscovery.GetIPAddresses())
            {
                Console.WriteLine($"  {a} is public {a.IsPublic()}");
            }


            Console.WriteLine();
            Console.WriteLine("Gateways");
            NatClient nat = null;
            foreach (var g in NatDiscovery.GetGateways())
            {
                bool q;
                Console.WriteLine($"  {g}");

                var pcp = new Makaretu.Nat.Pcp.Client(g);
                q = pcp.IsAvailableAsync().Result;
                Console.WriteLine($"    supports NAT-PCP {q}");
                if (q && nat == null)
                {
                    nat = pcp;
                }

                var pmp = new Makaretu.Nat.Pmp.Client(g);
                q = pmp.IsAvailableAsync().Result;
                Console.WriteLine($"    supports NAT-PMP {q}");
                if (q && nat == null)
                {
                    nat = pmp;
                }
            }

            if (nat != null)
            {
                Console.WriteLine();
                Console.WriteLine("Create public end point");
                using (var endpoint = nat.CreatePublicEndpointAsync(8080).Result)
                {
                    Console.WriteLine($"  public address '{endpoint}'");
                }
            }
        }
    }
}
