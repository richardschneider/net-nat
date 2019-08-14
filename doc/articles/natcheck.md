# NAT Check

## Install

The `natcheck` tool is available on [Nuget](https://www.nuget.org/packages/Makaretu.Nat/) and
is installed with dotnet.

    > dotnet tool install --global natcheck

## Usage

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
