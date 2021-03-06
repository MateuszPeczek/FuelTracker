﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace FuelTracker
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .CaptureStartupErrors(true)
                .UseStartup<Startup>()
                .Build();
    }
}
