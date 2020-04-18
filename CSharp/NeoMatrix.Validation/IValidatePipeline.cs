using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NeoMatrix.Validation
{
    public interface IValidatePipeline
    {
        Task<ValidateResult<bool>> ValidateAsync(Func<Task<HttpResponseMessage>> request);
    }
}