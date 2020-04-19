using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace NeoMatrix.Validation.Validators
{
    public static class ValidatorUtility
    {
        private readonly static Dictionary<ResultTypeEnum, IJsonTextValidator> _resultValidatorDict = new Dictionary<ResultTypeEnum, IJsonTextValidator>()
        {
            { ResultTypeEnum.None, FromDelegate((doc, _) => new ValidateResult<bool>(){ Result = true }) },
            { ResultTypeEnum.Number, FromDelegate(ValidateByNumber)},
            { ResultTypeEnum.String, FromDelegate(ValidateByString)},
            { ResultTypeEnum.Array, FromDelegate(ValidateByArray)},

            { ResultTypeEnum.True, FromDelegate((doc, _) => ValidateByBooleanValue(doc, true)) },
            { ResultTypeEnum.False, FromDelegate((doc, _) => ValidateByBooleanValue(doc, false)) },
            { ResultTypeEnum.Boolean, FromDelegate(ValidateByBoolean) },

            { ResultTypeEnum.Key, FromDelegate(ValidateByKeyOnly) },
            { ResultTypeEnum.KeyValuePair, FromDelegate(ValidateByKeyValuePair) },
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

        private static ValidateResult<bool> ValidateByArray(JsonDocument doc, string _)
        {
            return doc.RootElement.ValueKind == JsonValueKind.Array
                 ? new ValidateResult<bool>() { Result = true }
                 : new ValidateResult<bool>() { Result = false, ExtraErrorMsg = $"Invalid JsonValueKind: {doc.RootElement.ValueKind}. Should be Array" };
        }

        private static ValidateResult<bool> ValidateByString(JsonDocument doc, string value)
        {
            var resultValue = doc.RootElement.GetString();
            if (resultValue != value)
            {
                return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = $"String Value Not Matched: {resultValue}. Should be '{value}" };
            }
            return new ValidateResult<bool>() { Result = true };
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

        private static ValidateResult<bool> ValidateByKeyValuePair(JsonDocument doc, string pair)
        {
            var pairArray = pair.Split(":").Select(w => w.Replace(";", "")).ToArray();

            for (var i = 0; i < pairArray.Count(); i += 2)
            {
                string key = pairArray[i];
                string value = pairArray[i + 1];

                if (!doc.RootElement.TryGetProperty(key, out var element))
                {
                    return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = $"Property {key} Can't be Found." };
                }
                
                var resultValue = element.GetString();
                if (resultValue != value)
                {
                    return new ValidateResult<bool>() {
                        Result = false,
                        ExtraErrorMsg = $"Property Value Not Match: {resultValue}. Should be '{value}'"
                    };
                }
            }

            return new ValidateResult<bool>() { Result = true };
        }
    }
}