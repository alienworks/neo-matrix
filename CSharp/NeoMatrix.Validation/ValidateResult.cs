using System;

namespace NeoMatrix.Validation
{
    public struct ValidateResult<T>
    {
        public bool OK => Exception is null && string.IsNullOrEmpty(ExtraErrorMsg);

        public T Result { get; set; }

        public Exception Exception { get; set; }

        public string ExtraErrorMsg { get; set; }

        public ValidateResult<bool> ToBooleanValidateResult() => new ValidateResult<bool>() { Result = Result is bool r ? r : OK, Exception = Exception, ExtraErrorMsg = ExtraErrorMsg };
    }
}