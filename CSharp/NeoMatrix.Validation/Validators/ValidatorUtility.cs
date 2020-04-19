using System;
using System.Collections.Generic;
using System.Text.Json;

namespace NeoMatrix.Validation.Validators
{
    public static class ValidatorUtility
    {
        private readonly static Dictionary<ResultTypeEnum, IJsonTextValidator> _resultValidatorDict = new Dictionary<ResultTypeEnum, IJsonTextValidator>()
        {
            { ResultTypeEnum.None, FromDelegate((doc, _) => new ValidateResult<bool>(){ Result = true }) },
            { ResultTypeEnum.Number, FromDelegate(ValidateByNumber)},

            { ResultTypeEnum.True, FromDelegate((doc, _) => ValidateByBooleanValue(doc, true)) },
            { ResultTypeEnum.False, FromDelegate((doc, _) => ValidateByBooleanValue(doc, false)) },
            { ResultTypeEnum.Boolean, FromDelegate(ValidateByBoolean) },

            { ResultTypeEnum.Key, FromDelegate(ValidateByKeyOnly) },
        };

        public static IJsonTextValidator FromDelegate(Func<JsonDocument, string, ValidateResult<bool>> func) => new WrappedTextValidator(func);

        public static IJsonTextValidator GetCachedValidator(ResultTypeEnum resultType)
        {
            if (!_resultValidatorDict.TryGetValue(resultType, out var validator))
            {
                validator = _resultValidatorDict[ResultTypeEnum.None];
            }
            return validator;
        }

        private static ValidateResult<bool> ValidateByNumber(JsonDocument doc, string _)
        {
            return doc.RootElement.ValueKind == JsonValueKind.Number
                 ? new ValidateResult<bool>() { Result = true }
                 : new ValidateResult<bool>() { Result = false, ExtraErrorMsg = $"Invalid JsonValueKind: {doc.RootElement.ValueKind}. Should be Number" };
        }

        private static ValidateResult<bool> ValidateByBoolean(JsonDocument doc, string _)
        {
            var root = doc.RootElement;
            if (root.ValueKind != JsonValueKind.False && root.ValueKind != JsonValueKind.True)
            {
                return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = $"Invalid JsonValueKind: {root.ValueKind}. Should be Boolean" };
            }
            return new ValidateResult<bool>() { Result = true };
        }

        private static ValidateResult<bool> ValidateByBooleanValue(JsonDocument doc, bool value)
        {
            var root = doc.RootElement;
            if ((value && root.ValueKind == JsonValueKind.False) || (!value && root.ValueKind == JsonValueKind.True))
            {
                return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = $"Bool Value Not Matched: {root.ValueKind}. Should be '{value}'" };
            }
            return new ValidateResult<bool>() { Result = true };
        }

        private static ValidateResult<bool> ValidateByKeyOnly(JsonDocument doc, string word)
        {
            if (!doc.RootElement.TryGetProperty(word, out var _))
            {
                return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = $"Property {word} Not Found" };
            }
            return new ValidateResult<bool>() { Result = true };
        }
    }
}