﻿using System;
using System.ComponentModel.DataAnnotations;

namespace NeoMatrix.Data.Models
{
    public sealed class MatrixItemEntity
    {
        public long Id { get; set; }

        public string Url { get; set; }

        public string Net { get; set; }

        public string Method { get; set; }

        public byte Available { get; set; }

        public string Error { get; set; }

        public long GroupId { get; set; }

        public DateTime CreateTime { get; set; }
    }
}