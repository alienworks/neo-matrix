using System;
using NeoMatrix.Validation;
using NeoMatrix.Validation.Pipeline.Builders;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddValidateModule(this IServiceCollection services, Func<IHttpConnectionValidatePipelineBuilder, IValidatePipelineBuilder> configurePipelineBuilder)
        {
            var builder = configurePipelineBuilder(new HttpConnectionValidatePipelineBuilder());
            services.AddSingleton(builder);
        }
    }
}