using System;
using System.Text;

namespace NeoMatrix.Validation
{
    public struct ValidateResult<T>
    {
        public bool OK => Exception is null && string.IsNullOrEmpty(ExtraErrorMsg);

        public T Result { get; set; }

        public Exception Exception { get; set; }

        public string ExtraErrorMsg { get; set; }

        public ValidateResult<bool> ToBooleanValidateResult() => new ValidateResult<bool>() { Result = Result is bool r ? r : OK, Exception = Exception, ExtraErrorMsg = ExtraErrorMsg };

        public string ToFullErrorMsg()
        {
            if (OK)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder(32);
            var ex = Exception;
            if (ex != null)
            {
                sb.Append("Exp:");
                sb.Append(ex.Message);

                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    sb.Append('/');
                    sb.Append(ex.Message);
                }
            }
            
            if (!string.IsNullOrEmpty(ExtraErrorMsg))
            {
                sb.Append("|Extra:");
                sb.Append(ExtraErrorMsg);
            }
            return sb.ToString();
        }
    }
}