using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1028)]
    public class NotifyEquipmentSlotChanged
    {
        [ProtoMember(1)]
        public NetworkEquipmentSlot Slot { get; set; }
    }
}
