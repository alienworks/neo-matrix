using System;

namespace NeoMatrix.Data.Models
{
    public sealed class MatrixItemEntity
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Net { get; set; }

        public string Method { get; set; }

        public bool Available { get; set; }

        public string Error { get; set; }

        public int GroupId { get; set; }

        public DateTime CreateTime { get; set; }
    }
}