using Makaretu.Nat;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

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

            var endpoints = new List<LeasedEndpoint>();
            foreach (var nat in NatDiscovery.GetNats())
            { 
                Console.WriteLine();
                Console.WriteLine("Create public end point");
                var lease = nat.CreatePublicEndpointAsync(ProtocolType.Tcp, 8080).Result;
                var endpoint = new LeasedEndpoint(lease);
                endpoints.Add(endpoint);
                Console.WriteLine($"  public address '{endpoint}'");
            }

            Console.ReadLine();

            foreach (var endpoint in endpoints)
            {
                endpoint.Dispose();
            }
        }
    }
}
