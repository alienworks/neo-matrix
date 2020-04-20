using Microsoft.Extensions.Configuration;
using NeoMatrix;
using NeoMatrix.Configuration;
using NeoMatrix.HostedServices;
using NeoMatrix.Validation.Validators;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAppModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConfigurationOption>(configuration);

            services.AddHttpClient();

            services.AddSingleton<NodeCaller>();

            string rpcVersion = configuration.GetSection("rpcMethods").GetSection("common").GetValue<string>("jsonrpc");
            services.AddValidateModule(builder => builder
                    .UseConnectionValidator<DefaultHttpConnectionValidator>()
                    .UseContentValidator<DefaultHttpContentValidator>()
                    .Use(new HeadTextValidator() { VersionText = rpcVersion }));

            services.AddHostedService<RpcCheckHostedService>();
        }
    }
}