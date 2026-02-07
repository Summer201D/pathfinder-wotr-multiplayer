namespace WOTRMultiplayer.Entities.Content
{
    public class NetworkMod
    {
        public string Id { get; set; }

        public string Version { get; set; }

        public bool IsEnabled { get; set; }

        public NetworkModType Type { get; set; }
    }
}
