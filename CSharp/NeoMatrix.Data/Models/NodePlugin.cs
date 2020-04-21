using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace NeoMatrix.Data.Models
{
    public class NodePlugin
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string Url { get; set; }

        [JsonPropertyName("name")]
        public string PluginName { get; set; }
        public string Version { get; set; }
    }
}
