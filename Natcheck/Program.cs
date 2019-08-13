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
            foreach (var a in NatDiscovery.GetIPAddresses().OrderBy(a => a.AddressFamily))
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

            foreach (var nat in nats)
            {
                Lease lease = null;
                try
                {
                    lease = nat.CreatePublicEndpointAsync(ProtocolType.Tcp, 8080).Result;
                    Console.WriteLine($"  Public endpoint {lease.PublicAddress}:{lease.PublicPort}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"  Failed to create public endpoint. {e.Message}");
                }

                if (lease != null)
                {
                    try
                    {
                        nat.DeletePublicEndpointAsync(lease).Wait();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"  Failed to delete public endpoint. {e.Message}");
                    }
                }
            }
        }
    }
}
