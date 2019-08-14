# Prerequisites

The net-nat package requires other network components to be configured.

## Router

Your router must be configured to be a NAT that performs port forwarding to your computer.
As each router has a different administration UI, you will have to read the guide on your
router.  

Here's a good article on [port forwarding](https://www.howtogeek.com/66214/how-to-forward-ports-on-your-router/).

## Firewall

If your computer has a firewall (for example Windows Defender), you must also 
configure the firewall to open your local port.

Here's a good article on configuring [Windows Defender](https://www.howtogeek.com/394735/how-do-i-open-a-port-on-windows-firewall/).

## HTTP

If you are forwarding HTTP requests, then on Windows, `HTTP.SYS` must be configured to allow incoming requests
on port 80.

```console
> netsh http add urlacl url="http://+:80/" user="Everyone"
```