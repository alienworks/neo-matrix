using System;
using System.Text.Json;

namespace NeoMatrix.Validation.Validators
{
    internal sealed class WrappedTextValidator : IJsonTextValidator
    {
        private readonly Func<JsonDocument, string, ValidateResult<bool>> _func;

        public WrappedTextValidator(Func<JsonDocument, string, ValidateResult<bool>> func)
        {
            _func = func;
        }

        public ValidateResult<bool> Validate(JsonDocument doc, string words)
        {
            return _func.Invoke(doc, words);
        }
    }
}