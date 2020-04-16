namespace NeoMatrix.Validation
{
    public interface ITextValidator
    {
        ValidateResult<bool> Validate(string text);
    }
}