using System.Text.Json;

namespace NeoMatrix.Rpc
{
    internal sealed class LowCaseJsonNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) => name.ToLower();
    }
}