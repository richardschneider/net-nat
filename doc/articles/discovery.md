# NAT Discovery

[NatDiscover.GetNats()](xref:Makaretu.Nat.NatDiscovery.GetNats*) returns a 
sequence of [NatClient(s)](xref:Makaretu.Nat.NatClient) that can be used
to [create](xref:Makaretu.Nat.NatClient.CreatePublicEndpointAsync*) a public internet address.

The following steps are performed to discover a NAT

- use [GetGateways](xref:Makaretu.Nat.NatDiscovery.GetGateways*) to find the IP address of any network interface that has a gateway
- use [IsAvailableAsync](xref:Makaretu.Nat.NatClient.IsAvailableAsync*) to determine if the gateway supports PCP or PMP protocol
