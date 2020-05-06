using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NeoMatrix.Data;

namespace NeoMatrix
{
    internal class Program
    {
        private async static Task Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);
            hostBuilder.UseConsoleLifetime();
            using var host = hostBuilder.Build();

            using var serviceScope = host.Services.CreateScope();
            var services = serviceScope.ServiceProvider;
            var redisService = services.GetRequiredService<RedisService>();
            redisService.Connect();

            await StartMatrixSync(host);
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

        private static async Task StartMatrixSync(IHost host)
        {
            await host.StartAsync();
            await host.StopAsync();
        }

        private static async Task KeepMatrixSyncing(IHost host)
        {
            while (true)
            {
                await Task.Delay(10 * 1000);
                await StartMatrixSync(host);
            }
        }
    }
}