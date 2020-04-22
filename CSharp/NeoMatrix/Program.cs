using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NeoMatrix
{
    internal class Program
    {
        private async static Task Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);
            hostBuilder.UseConsoleLifetime();
            using var host = hostBuilder.Build();
            await host.StartAsync();
            // Console.ReadKey();
            await host.StopAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, configBuilder) =>
            {
                configBuilder
                //.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

                configBuilder.AddEnvironmentVariables();
                if (args != null)
                {
                    configBuilder.AddCommandLine(args);
                }
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddAppModule(hostContext.Configuration);
                services.AddDataModule(hostContext.Configuration);
            });
    }
}