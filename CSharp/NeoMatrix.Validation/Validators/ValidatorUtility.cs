using System;
using System.Collections.Generic;
using System.Text.Json;

namespace NeoMatrix.Validation.Validators
{
    public static class ValidatorUtility
    {
        private readonly static Dictionary<ResultTypeEnum, IJsonTextValidator> _resultValidatorDict = new Dictionary<ResultTypeEnum, IJsonTextValidator>()
        {
            { ResultTypeEnum.None, FromDelegate(ValidateByNone) },
            { ResultTypeEnum.Number, FromDelegate(ValidateByNumber)},
            { ResultTypeEnum.String, FromDelegate(ValidateByString)},
            { ResultTypeEnum.Array, FromDelegate(ValidateByArray)},

            { ResultTypeEnum.True, FromDelegate((doc, _) => ValidateByBooleanValue(doc, true)) },
            { ResultTypeEnum.False, FromDelegate((doc, _) => ValidateByBooleanValue(doc, false)) },
            { ResultTypeEnum.Boolean, FromDelegate(ValidateByBoolean) },

            { ResultTypeEnum.Key, FromDelegate(ValidateByKeyOnly) },
            { ResultTypeEnum.KeyValuePair, FromDelegate(ValidateByKeyValuePair) },
        };

        private static ValidateResult<bool> ValidateByNone(JsonDocument doc, string _) => ExistsResultProperty(doc, out var _)
            ? new ValidateResult<bool>() { Result = true }
            : new ValidateResult<bool>() { Result = false, ExtraErrorMsg = "Property 'result' Not Found." };

        private static bool ExistsResultProperty(JsonDocument doc, out JsonElement value) => doc.RootElement.TryGetProperty("result", out value);

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
            if (!ExistsResultProperty(doc, out var resultElement))
            {
                return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = "Property 'result' Not Found." };
            }

            return resultElement.ValueKind == JsonValueKind.Number
                 ? new ValidateResult<bool>() { Result = true }
                 : new ValidateResult<bool>() { Result = false, ExtraErrorMsg = $"Invalid JsonValueKind: {resultElement.ValueKind}. Should be Number" };
        }

        private static ValidateResult<bool> ValidateByArray(JsonDocument doc, string _)
        {
            if (!ExistsResultProperty(doc, out var resultElement))
            {
                return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = "Property 'result' Not Found." };
            }

            return resultElement.ValueKind == JsonValueKind.Array
                 ? new ValidateResult<bool>() { Result = true }
                 : new ValidateResult<bool>() { Result = false, ExtraErrorMsg = $"Invalid JsonValueKind: {resultElement.ValueKind}. Should be Array" };
        }

        private static ValidateResult<bool> ValidateByString(JsonDocument doc, string value)
        {
            if (!ExistsResultProperty(doc, out var resultElement))
            {
                return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = "Property 'result' Not Found." };
            }

            if (resultElement.ValueKind != JsonValueKind.String)
            {
                return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = $"Invalid JsonValueKind: {resultElement.ValueKind}. Should be String" };
            }

            var resultValue = resultElement.GetString();
            if (resultValue != value)
            {
                return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = $"String Value Not Matched: {resultValue}. Should be '{value}" };
            }
            return new ValidateResult<bool>() { Result = true };
        }

        private static ValidateResult<bool> ValidateByBoolean(JsonDocument doc, string _)
        {
            if (!ExistsResultProperty(doc, out var resultElement))
            {
                return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = "Property 'result' Not Found." };
            }

            if (resultElement.ValueKind != JsonValueKind.False && resultElement.ValueKind != JsonValueKind.True)
            {
                return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = $"Invalid JsonValueKind: {resultElement.ValueKind}. Should be Boolean" };
            }
            return new ValidateResult<bool>() { Result = true };
        }

        private static ValidateResult<bool> ValidateByBooleanValue(JsonDocument doc, bool value)
        {
            if (!ExistsResultProperty(doc, out var resultElement))
            {
                return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = "Property 'result' Not Found." };
            }

            if ((value && resultElement.ValueKind == JsonValueKind.False) || (!value && resultElement.ValueKind == JsonValueKind.True))
            {
                return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = $"Bool Value Not Matched: {resultElement.ValueKind}. Should be '{value}'" };
            }
            return new ValidateResult<bool>() { Result = true };
        }

        private static ValidateResult<bool> ValidateByKeyOnly(JsonDocument doc, string word)
        {
            if (!ExistsResultProperty(doc, out var resultElement))
            {
                return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = "Property 'result' Not Found." };
            }

            if (!resultElement.TryGetProperty(word, out var _))
            {
                return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = $"Property '{word}' Not Found" };
            }
            return new ValidateResult<bool>() { Result = true };
        }

        private static ValidateResult<bool> ValidateByKeyValuePair(JsonDocument doc, string word)
        {
            if (!ExistsResultProperty(doc, out var resultElement))
            {
                return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = "Property 'result' Not Found." };
            }

            string[] pairs = word.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (string pair in pairs)
            {
                string[] keyvalue = pair.Split(':', StringSplitOptions.RemoveEmptyEntries);
                if (keyvalue.Length != 2)
                {
                    continue;
                }
                string key = keyvalue[0].Trim();
                if (!resultElement.TryGetProperty(key, out var element))
                {
                    return new ValidateResult<bool>() { Result = false, ExtraErrorMsg = $"Property {key} Can't be Found." };
                }
                string value = keyvalue[1].Trim();
                string elementValue = element.GetString()?.Trim();
                if (elementValue != value)
                {
                    return new ValidateResult<bool>()
                    {
                        Result = false,
                        ExtraErrorMsg = $"Property Value Not Match: {elementValue}. Should be '{value}'"
                    };
                }
            }

            return new ValidateResult<bool>() { Result = true };
        }
    }
}