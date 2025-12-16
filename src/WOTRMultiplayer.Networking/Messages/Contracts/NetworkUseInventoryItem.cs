using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkUseInventoryItem
    {
        [ProtoMember(1)]
        public NetworkItem Item { get; set; }

        [ProtoMember(2)]
        public string UserUnitId { get; set; }

        [ProtoMember(3)]
        public NetworkTargetWrapper Target { get; set; }

        [ProtoMember(4)]
        public NetworkEquipmentSlotPosition SlotPosition { get; set; }
    }
}
