using Makaretu.Nat;
using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace NancyServer
{
    class Program
    {
        static void Main()
        {
            var hostConfigs = new HostConfiguration
            {
                UrlReservations = new UrlReservations() { CreateAutomatically = true }
            };
            using (var nancyHost = new NancyHost(new Uri("http://localhost:8888")))
            {
                nancyHost.Start();
                Console.WriteLine("listening on http://localhost:8888");

                var endpoints = new List<LeasedEndpoint>();
                foreach (var nat in NatDiscovery.GetNats())
                {
                    var lease = nat.CreatePublicEndpointAsync(ProtocolType.Tcp, 8888).Result;
                    var endpoint = new LeasedEndpoint(lease);
                    endpoints.Add(endpoint);
                    Console.WriteLine($"listening on public address '{endpoint}'");
                }

                Console.WriteLine("Press enter to stop");
                Console.ReadKey();

                foreach (var endpoint in endpoints)
                {
                    endpoint.Dispose();
                }
            }

            Console.WriteLine("Stopped. Good bye!");
        }
    }
}
