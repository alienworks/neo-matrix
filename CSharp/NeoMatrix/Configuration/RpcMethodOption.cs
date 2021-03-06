﻿using NeoMatrix.Validation;

namespace NeoMatrix.Configuration
{
    public sealed class RpcMethodOption
    {
        public string Name { get; set; }

        public object[] Params { get; set; }

        public string Result { get; set; }

        public ResultTypeEnum ResultType { get; set; }
    }
}