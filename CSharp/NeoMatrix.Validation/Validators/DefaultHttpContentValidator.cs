using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace NeoMatrix.Validation.Validators
{
    public sealed class DefaultHttpContentValidator : IHttpContentValidator
    {
        public async Task<ValidateResult<JsonDocument>> ValidateAsync(byte[] content)
        {
            JsonDocument doc;
            try
            {
                doc = JsonDocument.Parse(content, new JsonDocumentOptions() { AllowTrailingCommas = true });
            }
            catch (Exception ex)
            {
                return new ValidateResult<JsonDocument>() { Exception = ex, ExtraErrorMsg = "Invalid Json Format" };
            }
            return await Task.FromResult(new ValidateResult<JsonDocument>() { Result = doc });
        }
    }
}