using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.Json;
using System.IO;

namespace Spark
{
    public class Config
    {
        private static Config config = null;

        public static string Path = "config.json";
        public int SparkId { get; set; }
        public string LighthouseHost { get; set; }
        public Beacon[] Beacons { get; set; }

        private Config()
        {
            LighthouseHost = "https://kibilights.com";
            SparkId = 0;
            Beacons = new Beacon[]
            {
                new Beacon{ Id = 1, IP = "192.168.0.101"},
                new Beacon{ Id = 2, IP = "192.168.0.102"}
            };
        }

        public static Config Get()
        {
            if (config != null) return config;
            else throw new NullReferenceException("Config is not initialized. Use Load method.");
        }

        public static void Load()
        {
            string content = File.ReadAllText(Path);
            config = JsonSerializer.Deserialize<Config>(content);
        }

        public static void SaveDefault()
        {
            var conf = new Config();
            var options = new JsonSerializerOptions { WriteIndented = true};
            string serialized = JsonSerializer.Serialize<Config>(conf, options);
            File.WriteAllText(Path, serialized);
        }

        public string GetBeaconIP(int id)
        {
            var beacon = Beacons.FirstOrDefault(b => b.Id == id);
            if (beacon== null) throw new ArgumentOutOfRangeException();
            return beacon.IP;
        }
    }
}
