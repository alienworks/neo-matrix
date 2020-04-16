using System.Threading.Tasks;

namespace NeoMatrix.Validation
{
    public interface IHttpContentValidator
    {
        Task<ValidateResult<string>> ValidateAsync(byte[] content);
    }
}