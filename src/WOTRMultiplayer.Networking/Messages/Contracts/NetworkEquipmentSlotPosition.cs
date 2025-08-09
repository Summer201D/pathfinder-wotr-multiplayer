using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkEquipmentSlotPosition
    {
        [ProtoMember(1)]
        public string Type { get; set; }

        [ProtoMember(2)]
        public int Index { get; set; }
    }
}
