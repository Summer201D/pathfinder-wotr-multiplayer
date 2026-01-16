namespace WOTRMultiplayer.Entities.GlobalMap
{
    public class NetworkGlobalMapTravel
    {
        public NetworkGlobalMapPathType Type { get; set; }

        public NetworkGlobalMapTravelerMode Mode { get; set; }

        public NetworkGlobalMapLocation Destination { get; set; }

        public bool FromClick { get; set; }
    }
}
