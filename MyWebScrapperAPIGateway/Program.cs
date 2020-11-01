using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyWebScrapperAPIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                 })
             .ConfigureAppConfiguration((hostingContext, config) =>
             {
                 var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                 config.AddJsonFile($"ocelot.{env}.json", true, true);
             }).ConfigureLogging(logging => logging.AddConsole());
    }
}
