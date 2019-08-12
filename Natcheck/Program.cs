using Makaretu.Nat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Natcheck
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Check your NAT(s)");

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
                Console.WriteLine($"  {g}");

                bool q;
                var pcp = new Makaretu.Nat.Pcp.Client(g);
                q = pcp.IsAvailableAsync().Result;
                Console.WriteLine($"    supports NAT-PCP {q}");

                var pmp = new Makaretu.Nat.Pmp.Client(g);
                q = pmp.IsAvailableAsync().Result;
                Console.WriteLine($"    supports NAT-PMP {q}");

            }

            var nats = NatDiscovery.GetNats().ToArray();

            Console.WriteLine();
            Console.WriteLine("Nats");
            foreach (var nat in nats)
            {
                Console.Write($"  {nat.RemoteEndPoint} ");
                try
                {
                    var name = Dns.GetHostEntry(nat.RemoteEndPoint.Address).HostName;
                    Console.Write(name);
                }
                catch { }
                Console.WriteLine();
            }

            var endpoints = new List<LeasedEndpoint>();
            foreach (var nat in nats)
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
