using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkEquipmentSlot
    {
        [ProtoMember(1)]
        public NetworkEquipmentSlotPosition Position { get; set; }

        [ProtoMember(2)]
        public NetworkItem Item { get; set; }

        [ProtoMember(3)]
        public string OwnerId { get; set; }
    }
}
