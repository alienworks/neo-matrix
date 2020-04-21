﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NeoMatrix.Data.Models
{
    public class ValidationResult
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool OK { get; set; }
        public bool Result { get; set; }
        public string ExtraErrorMsg { get; set; }
    }
}