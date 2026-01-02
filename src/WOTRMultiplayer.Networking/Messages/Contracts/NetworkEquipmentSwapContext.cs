using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkEquipmentSwapContext
    {
        [ProtoMember(1)]
        public NetworkEquipmentSlotPosition From { get; set; }

        [ProtoMember(2)]
        public NetworkEquipmentSlotPosition To { get; set; }
    }
}
