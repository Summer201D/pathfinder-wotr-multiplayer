using WOTRMultiplayer.Entities.MapObjects;

namespace WOTRMultiplayer.Entities.Inspect
{
    public class NetworkPerceptionCheck
    {
        public string UnitId { get; set; }

        public NetworkMapObject MapObject { get; set; }

        public int Roll { get; set; }
    }
}
