using System;

namespace NeoMatrix.Validation
{
    public interface IValidatePipelineBuilder
    {
        IHttpContentValidatePipelineBuilder AddConnectionValidator(IHttpConnectionValidator validator);
    }

    public interface IHttpContentValidatePipelineBuilder
    {
        ITextValidatePipelineBuilder Use(IHttpContentValidator validator);
    }

    public interface ITextValidatePipelineBuilder
    {
        ITextValidatePipelineBuilder Use(ITextValidator validator);

        ITextValidatePipelineBuilder Use(Func<string, bool> validateFunc);
    }
}