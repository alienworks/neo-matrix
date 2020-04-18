using System.Text.Json;

namespace NeoMatrix.Validation
{
    public interface IJsonTextValidator
    {
        ValidateResult<bool> Validate(JsonDocument doc, string words = null);
    }
}