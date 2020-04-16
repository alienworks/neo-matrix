using System;

namespace NeoMatrix.Validation
{
    public struct ValidateResult<T>
    {
        public T Result { get; set; }

        public Exception Exception { get; set; }

        public string ExtraErrorMsg { get; set; }
    }
}