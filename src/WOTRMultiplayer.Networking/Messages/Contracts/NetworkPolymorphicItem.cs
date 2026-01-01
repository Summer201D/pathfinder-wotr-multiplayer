using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkPolymorphicItem
    {
        [ProtoMember(1)]
        public string UnitId { get; set; }

        [ProtoMember(2)]
        public NetworkEquipmentSlotPosition Position { get; set; }

        [ProtoMember(3)]
        public NetworkItem Item { get; set; }
    }
}
