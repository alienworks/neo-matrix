using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NeoMatrix.Caches;
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
        private readonly int[] _indexesOption;
        private readonly IMatrixCache _matrixCache;

        public NodeCaller(
            IHttpClientFactory clientFactory,
            IValidatePipelineBuilder pipelineBuilder,
            IMatrixCache matrixCache,
            IOptions<ConfigurationOption> option)
        {
            _indexesOption = option.Value.Indexes;
            _commonOption = option.Value.RpcMethods.Common ?? throw new ArgumentNullException(nameof(CommonMethodOption));
            _matrixCache = matrixCache;
            _rpcMethods = option.Value.RpcMethods.Items.ToList()
                .Where((_, i) => _indexesOption.Any(index => index == i))
                .Select(method =>
                {
                    method.Name = method.Name.ToLower();
                    return method;
                }).ToArray() 
                ?? throw new ArgumentNullException(nameof(RpcMethodOptions));

            // _rpcMethods = new RpcMethodOption[] { new RpcMethodOption() { Name = "getblocksysfee", Params = new object[] { 1005434 } } };

            _clientFactory = clientFactory;
            _pipelineBuilder = pipelineBuilder;
        }

        public async Task<Node> ExecuteAsync(Node node)
        {
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(node.Url);
            
            var tasks = _rpcMethods.Select(async m =>
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
                      var body = new RpcRequestBody(m.Name)
                      {
                          JsonRpc = _commonOption.Jsonrpc,
                          Params = m.Params ?? Array.Empty<object>(),
                          Id = 1
                      };
                      string bodyStr = JsonSerializer.Serialize(body, jsonSerializerOptions);
                      var httpContent = new StringContent(bodyStr, Encoding.UTF8, "application/json");
                      return await client.PostAsync(string.Empty, httpContent);
                  }, m.Result ?? string.Empty);

                await _matrixCache.CreateValidationAsync(new ValidationResult
                {
                    Name = m.Name,
                    OK = r.OK,
                    ExtraErrorMsg = r.ExtraErrorMsg,
                    Url = node.Url,
                    Result = r.Result
                });
            });
            await Task.WhenAll(tasks);
            return node;
        }

        public async Task<NodePlugin[]> GetPluginsAsync(Node node)
        {
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(node.Url);

            var body = new RpcRequestBody("listplugins") { 
                JsonRpc = _commonOption.Jsonrpc, 
                Params = Array.Empty<object>(), 
                Id = 1 
            };

            string bodyStr = JsonSerializer.Serialize(body, jsonSerializerOptions);
            var httpContent = new StringContent(bodyStr, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(string.Empty, httpContent);

            var rspStream = await response.Content.ReadAsStreamAsync();

            var repBody = await JsonSerializer.DeserializeAsync<RpcResponseBody<IEnumerable<NodePlugin>>>(rspStream);

            foreach (var b in repBody.Result)
            {
                Console.WriteLine(b.Id);
            }

            return default;
        }
    }
}