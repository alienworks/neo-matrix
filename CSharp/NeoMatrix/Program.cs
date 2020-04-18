using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NeoMatrix
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, configBuilder) =>
            {
                configBuilder.AddJsonFile("appsettings.json");
                configBuilder.AddEnvironmentVariables();
                if (args != null)
                {
                    configBuilder.AddCommandLine(args);
                }
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddAppModule(hostContext.Configuration);
            });
    }
}