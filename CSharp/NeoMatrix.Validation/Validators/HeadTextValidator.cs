using System.Text.Json;

namespace NeoMatrix.Validation.Validators
{
    public sealed class HeadTextValidator : IJsonTextValidator
    {
        public string VersionText { get; set; } = "2.0";

        public ValidateResult<bool> Validate(JsonDocument doc, string _)
        {
            var root = doc.RootElement;
            if (!root.TryGetProperty("jsonrpc", out var jsonrpc))
            {
                return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = "Property 'jsonrpc' Not Found" };
            }
            string version = jsonrpc.GetString();
            if (version != VersionText)
            {
                return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = $"Property 'jsonrpc' Value Not Matched: {version}. Shoud be {VersionText}" };
            }
            if (!root.TryGetProperty("id", out var id))
            {
                return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = "Property 'id' Not Found" };
            }
            if (id.ValueKind != JsonValueKind.Number)
            {
                return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = $"Property 'id' Type Not Matched: {id.ValueKind}" };
            }
            //if (!root.TryGetProperty("result", out var _))
            //{
            //    return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = "Property 'result' Not Found" };
            //}
            return new ValidateResult<bool>() { Result = true };
        }
    }
}