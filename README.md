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

## Getting started

Published releases are available on [NuGet](https://www.nuget.org/packages/Makaretu.Nat/).  To install, run the following command in the [Package Manager Console](https://docs.nuget.org/docs/start-here/using-the-package-manager-console).

    PM> Install-Package Makaretu.Nat
    
