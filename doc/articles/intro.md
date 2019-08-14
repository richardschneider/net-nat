# Makaretu NAT

The main purpose of this package is to allow a server behind a NAT to accept connections from the public internet.
This allows any internet user to access your home server.

The basic idea is that your home NAT (router/gateway) is asked to create an IP address
this is publically addressable.  The NAT then forwards any messsages to the public address
to your local host.

The source code is on [GitHub](https://github.com/richardschneider/net-nat) and the 
package is published on [NuGet](https://www.nuget.org/packages/Makaretu.Nat). 


It uses the following protocols to create a public IP address

- [RFC 6887 - NAT Port Control Protocol](https://tools.ietf.org/html/rfc6887)
- [RFC 6886 - NAT Port Mapping Protocol](https://tools.ietf.org/html/rfc6886)

> [!WARNING]
>  NAT Port Mapping Protocol is not fully implemented.  See [issue #3](https://github.com/richardschneider/net-nat/issues/3).

Use the [natcheck](natcheck.md) tool to examine your network environment. See the [gotchas]()
for reasons why things fail.