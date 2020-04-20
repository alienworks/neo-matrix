using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using NeoMatrix.Data.Models;

namespace NeoMatrix.HostedServices.Seeds
{
    internal sealed class NodeSeedsLoader
    {
        private static string SeedJsonFileNameFormat = "seed-{0}.json";

        public static List<Node> Load()
        {
            var result = LoadFromJsonFile(Constants.MAIN_NET, Constants.TEST_NET);
            return result;
        }

        public static List<Node> LoadFromJsonFile(params string[] nets)
        {
            var result = new List<Node>(64);
            foreach (string net in nets)
            {
                string file = Path.Join("Resources", string.Format(SeedJsonFileNameFormat, net.ToLower()));

                Console.WriteLine(file);

                var bytes = File.ReadAllBytes(file);
                var temp = JsonSerializer.Deserialize<List<Node>>(bytes, new JsonSerializerOptions() { AllowTrailingCommas = true, PropertyNameCaseInsensitive = true });
                temp.ForEach(n => n.Net = net);
                result.AddRange(temp);
            }
            
            return result;
        }
    }
}