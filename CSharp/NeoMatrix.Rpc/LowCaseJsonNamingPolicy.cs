using System.Text.Json;

namespace NeoMatrix.Rpc
{
    public sealed class LowCaseJsonNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) => name.ToLower();
    }
}