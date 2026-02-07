namespace WOTRMultiplayer.Entities.Content
{
    public class NetworkDiscrepantMod
    {
        public string Id { get; set; }

        public NetworkModType Type { get; set; }

        public string HostVersion { get; set; }

        public string Version { get; set; }

        public NetworkDiscrepancyReason Reason { get; set; }

        public NetworkDiscrepantMod(string id, NetworkModType type, string version, string hostVersion, NetworkDiscrepancyReason reason)
        {
            Id = id;
            Type = type;
            HostVersion = hostVersion;
            Version = version;
            Reason = reason;
        }

        public NetworkDiscrepantMod()
        {
        }
    }
}
