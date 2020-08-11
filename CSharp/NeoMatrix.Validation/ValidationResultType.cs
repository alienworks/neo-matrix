namespace NeoMatrix.Validation
{
    public enum ValidationResultType : byte
    {
        /// <summary>
        /// 不可用
        /// </summary>
        Unavailable = 0,

        /// <summary>
        /// 可用
        /// </summary>
        Available = 1,

        /// <summary>
        /// 未校验
        /// </summary>
        Unchecked = 2
    }
}