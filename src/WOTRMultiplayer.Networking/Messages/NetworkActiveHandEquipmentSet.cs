using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages
{
    [ProtoContract]
    public class NetworkActiveHandEquipmentSet
    {
        [ProtoMember(1)]
        public string UnitId { get; set; }

        [ProtoMember(2)]
        public int Index { get; set; }
    }
}
