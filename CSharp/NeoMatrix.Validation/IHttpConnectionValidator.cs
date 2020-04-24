using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NeoMatrix.Validation
{
    public interface IHttpConnectionValidator
    {
        Task<ValidateResult<byte[]>> ValidateAsync(Func<Task<HttpResponseMessage>> action);
    }
}