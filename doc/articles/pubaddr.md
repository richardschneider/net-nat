# Public Address

A public address is an IP address that is reachable from other hosts that do not
belong to your private network.  That NAT will not permanently assign a public address,
but will support it for a time period, see the [Lease](xref:Makaretu.Nat.Lease) class.  

The [LeasedEndpoint](xref:Makaretu.Nat.LeasedEndpoint) is used to renew the
public address.

## Creating

Ask the NAT to create a [public address](xref:Makaretu.Nat.NatClient.CreatePublicEndpointAsync*) and then create a leased endpoint.

```csharp
NatClient nat = ...
var lease = await nat.CreatePublicEndpointAsync(ProtocolType.Tcp, 8080);
var endpoint = new LeasedEndpoint(lease);
```

## Renewal

The [LeasedEndpoint](xref:Makaretu.Nat.LeasedEndpoint) continuously renews the lease on the public address when the
life time is about to expire. 

The NAT can change the public address or port when a renewal occurs!
The [Change event](xref:Makaretu.Nat.LeasedEndpoint.Changed) is raised when this occurs.  You can then use this to inform
others of your new public address.
 
## Disposal

It is good practice to tell the NAT that the leased endpoint is no longer needed.
Use the [Dispose](xref:Makaretu.Nat.LeasedEndpoint.Dispose*) method.
