﻿using System;
using System.Text.Json;

namespace NeoMatrix.Validation
{
    public interface IHttpConnectionValidatePipelineBuilder
    {
        IHttpContentValidatePipelineBuilder UseConnectionValidator<T>() where T : IHttpConnectionValidator, new();

        IHttpContentValidatePipelineBuilder Use(IHttpConnectionValidator validator);
    }

    public interface IHttpContentValidatePipelineBuilder
    {
        IValidatePipelineBuilder UseContentValidator<T>() where T : IHttpContentValidator, new();

        IValidatePipelineBuilder Use(IHttpContentValidator validator);
    }

    /// <summary>
    /// WARNING: For concurrently use, every Use() should return a new instance of IValidatePipelineBuilder
    /// </summary>
    public interface IValidatePipelineBuilder
    {
        IValidatePipelineBuilder UseJsonTextValidator<T>() where T : IJsonTextValidator, new();

        IValidatePipelineBuilder Use(IJsonTextValidator validator);

        IValidatePipelineBuilder Use(Func<JsonDocument, string, ValidateResult<bool>> validateFunc);

        IValidatePipeline Build();
    }
}