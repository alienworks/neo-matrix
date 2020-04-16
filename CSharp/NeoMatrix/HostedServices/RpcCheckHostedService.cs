using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NeoMatrix.HostedServices
{
    internal sealed class RpcCheckHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public RpcCheckHostedService(IConfiguration configuration,
            ILogger<RpcCheckHostedService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            string a = _configuration.GetValue<string>("Test");
            _logger.LogInformation(a);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
