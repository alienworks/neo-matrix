﻿using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using NeoMatrix.Data.Models;

namespace NeoMatrix
{
    internal sealed class NodeSeedsLoader
    {
        private const string SeedJsonFileNameFormat = "seed-{0}.json";

        public List<Node> Load()
        {
            var result = LoadFromJsonFile(Constants.MAIN_NET, Constants.TEST_NET);
            return result;
        }

        private List<Node> LoadFromJsonFile(params string[] nets)
        {
            var result = new List<Node>(64);
            var options = new JsonSerializerOptions() { AllowTrailingCommas = true, PropertyNameCaseInsensitive = true };
            foreach (string net in nets)
            {
                string filePath = Path.Combine("Resources", string.Format(SeedJsonFileNameFormat, net.ToLower()));
                var bytes = File.ReadAllBytes(filePath);
                var temp = JsonSerializer.Deserialize<List<Node>>(bytes, options);
                temp.ForEach(n => n.Net = net);
                result.AddRange(temp);
            }
            return result;
        }
    }
}