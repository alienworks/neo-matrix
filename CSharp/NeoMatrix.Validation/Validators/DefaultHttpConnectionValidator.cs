using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NeoMatrix.Validation.Validators
{
    public sealed class DefaultHttpConnectionValidator : IHttpConnectionValidator
    {
        public async Task<ValidateResult<byte[]>> ValidateAsync(Func<Task<HttpResponseMessage>> action)
        {
            HttpResponseMessage rsp = null;
            try
            {
                rsp = await action.Invoke();
            }
            catch (Exception ex)
            {
                rsp?.Dispose();
                return new ValidateResult<byte[]>() { Result = null, Exception = ex };
            }
            if (!rsp.IsSuccessStatusCode)
            {
                return new ValidateResult<byte[]>() { Result = null, ExtraErrorMsg = $"Invalid HttpStatusCode: {(int)rsp.StatusCode}" };
            }
            var bytes = await rsp.Content.ReadAsByteArrayAsync();
            return new ValidateResult<byte[]>() { Result = bytes };
        }
    }
}