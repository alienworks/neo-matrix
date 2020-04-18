using System;
using System.Text.Json;

namespace NeoMatrix.Validation.Validators
{
    internal sealed class WrappedTextValidator : IJsonTextValidator
    {
        private readonly Func<JsonDocument, ValidateResult<bool>> _func;

        public WrappedTextValidator(Func<JsonDocument, ValidateResult<bool>> func)
        {
            _func = func;
        }

        public ValidateResult<bool> Validate(JsonDocument doc)
        {
            return _func.Invoke(doc);
        }
    }
}