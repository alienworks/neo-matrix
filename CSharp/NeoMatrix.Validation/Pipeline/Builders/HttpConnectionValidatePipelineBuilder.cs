using System;
using System.Collections.Generic;
using System.Text.Json;
using NeoMatrix.Validation.Validators;

namespace NeoMatrix.Validation.Pipeline.Builders
{
    internal sealed class HttpConnectionValidatePipelineBuilder : IHttpConnectionValidatePipelineBuilder
    {
        public IHttpContentValidatePipelineBuilder Use(IHttpConnectionValidator validator)
        {
            return new HttpContentValidatePipelineBuilder(validator);
        }

        public IHttpContentValidatePipelineBuilder UseConnectionValidator<T>() where T : IHttpConnectionValidator, new()
        {
            return Use(new T());
        }
    }

    internal sealed class HttpContentValidatePipelineBuilder : IHttpContentValidatePipelineBuilder
    {
        public HttpContentValidatePipelineBuilder(IHttpConnectionValidator validator)
        {
            ConnectionValidator = validator;
        }

        public IHttpConnectionValidator ConnectionValidator { get; }

        public IValidatePipelineBuilder Use(IHttpContentValidator validator)
        {
            return new DefaultValidatePipelineBuilder(ConnectionValidator, validator);
        }

        public IValidatePipelineBuilder UseContentValidator<T>() where T : IHttpContentValidator, new()
        {
            return Use(new T());
        }
    }

    internal sealed class DefaultValidatePipelineBuilder : IValidatePipelineBuilder
    {
        public IHttpConnectionValidator ConnectionValidator { get; set; }

        public IHttpContentValidator ContentValidator { get; set; }

        public LinkedList<IJsonTextValidator> TextValidators { get; set; }

        public DefaultValidatePipelineBuilder(IHttpConnectionValidator connectionValidator, IHttpContentValidator validator)
        {
            ConnectionValidator = connectionValidator;
            ContentValidator = validator;
        }

        public IValidatePipelineBuilder Use(IJsonTextValidator validator)
        {
            var builder = new DefaultValidatePipelineBuilder(ConnectionValidator, ContentValidator)
            {
                TextValidators = TextValidators is null ? new LinkedList<IJsonTextValidator>() : new LinkedList<IJsonTextValidator>(TextValidators)
            };
            builder.TextValidators.AddLast(validator);
            return builder;
        }

        public IValidatePipelineBuilder UseJsonTextValidator<T>() where T : IJsonTextValidator, new()
        {
            return Use(new T());
        }

        public IValidatePipelineBuilder Use(Func<JsonDocument, ValidateResult<bool>> validateFunc)
        {
            var validator = ValidatorUtility.FromDelegate(validateFunc);
            return Use(validator);
        }

        public IValidatePipeline Build()
        {
            return new DefaultValidatePipeline()
            {
                ConnectionValidator = ConnectionValidator,
                ContentValidator = ContentValidator,
                TextValidators = TextValidators,
            };
        }
    }
}