using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NeoMatrix.Data;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDataModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MatrixDbContext>(opt =>
            {
                opt.UseMySql(configuration.GetConnectionString("DefaultConnection"), builder =>
                {
                    builder
                    .EnableRetryOnFailure(3)
                    .CommandTimeout(3);
                });
            });
        }
    }
}