using Nancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace NancyServer
{
    public class HelloModule : NancyModule
    {
        public HelloModule()
        {
            Get("/", _ => "Hello World!");
        }
    }
}
