namespace WOTRMultiplayer.Entities
{
    public class NetworkTargetWrapper
    {
        public NetworkVector3 Point { get; set; }

        public float? Orientation { get; set; }

        public string UnitId { get; set; }

        public NetworkTargetWrapper(NetworkVector3 point, float? orientation, string unitId)
        {
            Point = point;
            Orientation = orientation;
            UnitId = unitId;
        }

        public NetworkTargetWrapper()
        {
        }
    }
}
