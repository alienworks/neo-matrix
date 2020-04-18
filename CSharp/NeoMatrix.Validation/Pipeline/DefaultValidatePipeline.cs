using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NeoMatrix.Validation.Pipeline
{
    internal sealed class DefaultValidatePipeline : IValidatePipeline
    {
        public IHttpConnectionValidator ConnectionValidator { get; internal set; }

        public IHttpContentValidator ContentValidator { get; internal set; }

        public LinkedList<IJsonTextValidator> TextValidators { get; internal set; }

        public async Task<ValidateResult<bool>> ValidateAsync(Func<Task<HttpResponseMessage>> request, string word = null)
        {
            var connResult = await ConnectionValidator.ValidateAsync(request);
            if (!connResult.OK)
            {
                return connResult.ToBooleanValidateResult();
            }
            var contentResult = await ContentValidator.ValidateAsync(connResult.Result);
            if (!contentResult.OK)
            {
                return contentResult.ToBooleanValidateResult();
            }
            using var jsonDoc = contentResult.Result;
            foreach (var tv in TextValidators)
            {
                var textResult = tv.Validate(jsonDoc, word);
                if (!textResult.Result)
                {
                    return textResult;
                }
            }
            return new ValidateResult<bool>() { Result = true };
        }
    }
}