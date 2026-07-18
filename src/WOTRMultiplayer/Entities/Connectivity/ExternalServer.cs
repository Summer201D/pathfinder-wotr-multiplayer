using Newtonsoft.Json;

namespace WOTRMultiplayer.Entities.Connectivity
{
    public class ExternalServer
    {
        public string Url { get; set; }

        public string Name { get; set; }

        public string Prefix { get; set; }

        public string GameHubPath { get; set; }

        [JsonIgnore]
        public bool IsFake { get; set; }
    }
}
