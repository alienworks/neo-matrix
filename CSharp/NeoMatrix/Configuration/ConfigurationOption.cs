using System.Text.Json.Serialization;

namespace NeoMatrix.Configuration
{
    public sealed class ConfigurationOption
    {
        public RetryPolicyOption RetryPolicy { get; set; }

        public RpcMethodOptions RpcMethods { get; set; }

        public int[] Indexes { get; set; }
    }

    public sealed class RetryPolicyOption
    {
        public string Use { get; set; }
    }
}