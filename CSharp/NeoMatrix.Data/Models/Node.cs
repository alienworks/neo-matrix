using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NeoMatrix.Data.Models
{
    public sealed class Node
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public string Url { get; set; }
        public string Net { get; set; }

        public ICollection<ValidationResult> ValidationResults { get; set; }

    }
}