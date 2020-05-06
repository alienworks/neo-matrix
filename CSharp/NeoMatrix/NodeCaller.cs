using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NeoMatrix.Configuration;
using NeoMatrix.Data.Models;
using NeoMatrix.Rpc;
using NeoMatrix.Validation;
using NeoMatrix.Validation.Validators;

namespace NeoMatrix
{
    public sealed class NodeCaller
    {
        private readonly static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            PropertyNamingPolicy = new LowCaseJsonNamingPolicy()
        };

        private readonly IHttpClientFactory _clientFactory;
        private readonly IValidatePipelineBuilder _pipelineBuilder;
        private readonly CommonMethodOption _commonOption;
        private readonly RpcMethodOption[] _rpcMethods;

        public NodeCaller(
            IHttpClientFactory clientFactory, 
            IValidatePipelineBuilder pipelineBuilder, 
            IOptions<CommonMethodOption> commonOption, 
            IOptions<RpcMethodOptions> methodOptions)
        {
            _commonOption = commonOption.Value ?? throw new ArgumentNullException(nameof(CommonMethodOption));
            var allRpcMethods = methodOptions.Value?.Items ?? throw new ArgumentNullException(nameof(RpcMethodOptions));
            var indexes = methodOptions.Value.Indexes ?? throw new ArgumentNullException(nameof(RpcMethodOptions) + '_' + nameof(RpcMethodOptions.Indexes));
            _rpcMethods = indexes.Select(i => allRpcMethods[i]).ToArray();

            _clientFactory = clientFactory;
            _pipelineBuilder = pipelineBuilder;
        }

        public async Task<NodeCache> ExecuteAsync(Node node)
        {
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(node.Url);
            client.Timeout = TimeSpan.FromMilliseconds(_commonOption.Timeout);
            var result = new NodeCache(node);
            var tasks = _rpcMethods.AsParallel().Select(async m =>
            {
                IValidatePipeline pipeline;
                if (m.ResultType != ResultTypeEnum.None)
                {
                    var resultValidator = ValidatorUtility.GetCachedValidator(m.ResultType);
                    pipeline = _pipelineBuilder.Use(resultValidator).Build();
                }
                else
                {
                    pipeline = _pipelineBuilder.Build();
                }
                var r = await pipeline.ValidateAsync(async () =>
                  {
                      var body = new RpcRequestBody(m.Name.ToLower())
                      {
                          JsonRpc = _commonOption.Jsonrpc,
                          Params = m.Params ?? Array.Empty<object>(),
                          Id = _commonOption.Id
                      };
                      string bodyStr = JsonSerializer.Serialize(body, jsonSerializerOptions);
                      var httpContent = new StringContent(bodyStr, Encoding.UTF8, "application/json");
                      return await client.PostAsync(string.Empty, httpContent);
                  }, m.Result ?? string.Empty);
                result.MethodsResult.TryAdd(m.Name, r);
            });
            await Task.WhenAll(tasks);
            return result;
        }
    }
}