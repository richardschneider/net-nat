# Server Example

[Nancy Server](https://github.com/richardschneider/net-nat/tree/master/NancyServer) is a sample
project that demostrates a simple [nancy](http://nancyfx.org/) server that creates a public address.

## Setup

On Windows, `HTTP.SYS` must be configured to allow incoming requests
on port 80.

> netsh http add urlacl url="http://+:80/" user="Everyone"

## Testing locally

Run the server in a different process
```
> start dotnet run --project NancyServer

listening on http://localhost:80
listening on public address '165.84.19.45:80'
Press enter to stop
```

Get a page from the server
```
> curl http://localhost

Hello World!
```

## Testing publicly

Run the server in a different process
```
> start dotnet run --project NancyServer

listening on http://localhost:80
listening on public address '165.84.19.45:80'
Press enter to stop
```

In your browser, go to a web proxy, such as [proxysite.com](https://www.proxysite.com/),
and enter the public address (in this case `165.84.19.45`).
