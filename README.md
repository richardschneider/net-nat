# net-nat

[![build status](https://ci.appveyor.com/api/projects/status/github/richardschneider/net-nat?branch=master&svg=true)](https://ci.appveyor.com/project/richardschneider/net-nat) 
[![travis build](https://travis-ci.org/richardschneider/net-nat.svg?branch=master)](https://travis-ci.org/richardschneider/net-nat)
[![Coverage Status](https://coveralls.io/repos/richardschneider/net-nat/badge.svg?branch=master&service=github)](https://coveralls.io/github/richardschneider/net-nat?branch=master)
[![Version](https://img.shields.io/nuget/v/Makaretu.Nat.svg)](https://www.nuget.org/packages/Makaretu.Nat)
[![docs](https://cdn.rawgit.com/richardschneider/net-nat/master/doc/images/docs-latest-green.svg)](https://richardschneider.github.io/net-nat/articles/intro)

Life behind a NAT (Network Address Translator).

The main purpose of this package is to allow a server behind a NAT to accept connections from the public internet.

It uses the following protocols to create a public IP address

- [RFC 6887 - NAT Port Control Protocol](https://tools.ietf.org/html/rfc6887)
- [RFC 6886 - NAT Port Mapping Protocol](https://tools.ietf.org/html/rfc6886)

It supports the following runtimes

- .NET Framework 4.5
- .NET Standard 2.0

More information is on the [Documentation](https://richardschneider.github.io/net-nat/) web site.

## Library

### Getting started

Published releases are available on [NuGet](https://www.nuget.org/packages/Makaretu.Nat/).  To install, run the following command in the [Package Manager Console](https://docs.nuget.org/docs/start-here/using-the-package-manager-console).

    PM> Install-Package Makaretu.Nat
    
Or using [dotnet](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet?tabs=netcore21)

    > dotnet add package Makaretu.Nat

### Usage

Find the NAT(s) and create a public address, [LeasedEndPoint](https://richardschneider.github.io/net-nat/api/Makaretu.Nat.LeasedEndpoint.html).

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

## Tool

### Install

The `natcheck` tool is available on [Nuget]() and
is installed with dotnet.

    > dotnet tool install --global natcheck

### Usage

`natcheck` is a tool to examine the environment and display the network settings.

> natcheck

```
Check your NAT(s)

Unicast addresses
  169.254.253.248 is public False
  192.168.178.21 is public False
  fe80::8891:f4b8:3f8:fdf8%10 is public False
  2406:e001:xxxx:f601:7573:b0a8:46b0:xxxx is public True
  2406:e001:xxxx:f601:49f5:3a68:e240:xxxx is public True
  fe80::7573:xxxx:46b0:xxxx%11 is public False

Gateways
  fe80::3a10:d5ff:fe09:1c0e%11
    supports NAT-PCP False
    supports NAT-PMP False
  192.168.178.1
    supports NAT-PCP True
    supports NAT-PMP False

Nats
  192.168.178.1:5351 fritz.box
  Public endpoint 165.84.19.45:8080
```
