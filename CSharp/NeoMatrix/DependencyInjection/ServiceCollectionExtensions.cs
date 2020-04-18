using Microsoft.Extensions.Configuration;
using NeoMatrix;
using NeoMatrix.HostedServices;
using NeoMatrix.Validation.Validators;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAppModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();

            services.AddSingleton<NodeCaller>();

            string rpcVersion = configuration.GetSection("Common").GetValue<string>("jsonrpc");
            services.AddValidateModule(builder => builder
                    .UseConnectionValidator<DefaultHttpConnectionValidator>()
                    .UseContentValidator<DefaultHttpContentValidator>()
                    .Use(new HeadTextValidator() { VersionText = rpcVersion }));

            services.AddHostedService<RpcCheckHostedService>();
        }
    }
}