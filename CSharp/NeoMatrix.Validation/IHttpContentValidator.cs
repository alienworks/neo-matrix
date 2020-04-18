using System.Text.Json;
using System.Threading.Tasks;

namespace NeoMatrix.Validation
{
    public interface IHttpContentValidator
    {
        Task<ValidateResult<JsonDocument>> ValidateAsync(byte[] content);
    }
}