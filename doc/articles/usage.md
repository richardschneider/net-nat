# Usage

The general pattern is to 
- find the NAT(s), [NatDiscovery.GetNats](xref:Makaretu.Nat.NatDiscovery.GetNats*)
- create a public address on the NAT, [LeasedEndPoint](xref:Makaretu.Nat.LeasedEndpoint).
- inform others of the public address


```csharp
using Makaretu.Nat;
using System.Net.Sockets;

var endpoints = new List<LeasedEndpoint>();
foreach (var nat in NatDiscovery.GetNats())
{ 
    var lease = await nat.CreatePublicEndpointAsync(ProtocolType.Tcp, 8080);
    var endpoint = new LeasedEndpoint(lease);
    endpoints.Add(endpoint);
    Console.WriteLine($"Public address {endpoint}");
}

// Do your work.

foreach (var endpoint in endpoints)
{
    endpoint.Dispose();
}

```
