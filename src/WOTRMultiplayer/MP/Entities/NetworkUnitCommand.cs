using System.Numerics;

namespace WOTRMultiplayer.MP.Entities
{
    public class NetworkUnitCommand
    {
        public NetworkUnitCommandType CommandType { get; set; }

        public string UnitId { get; set; }

        public string TargetId { get; set; }

        public Vector3 Destination { get; set; }

        public float? Orientation { get; set; }

        public bool CreatedByPlayer { get; set; }
    }


    public enum NetworkUnitCommandType
    {
        Ignored, // should be skipped

        Unknown, // should be reviewed & implemented
        Move,
        Attack
    }
}
