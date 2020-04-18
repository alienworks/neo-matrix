using System;
using System.Text.Json;

namespace NeoMatrix.Validation.Validators
{
    internal static class ValidatorUtility
    {
        public static IJsonTextValidator FromDelegate(Func<JsonDocument, ValidateResult<bool>> func) => new WrappedTextValidator(func);
    }
}