using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1029)]
    public class NotifyActiveHandEquipmentSetChanged
    {
        [ProtoMember(1)]
        public NetworkActiveHandEquipmentSet Set { get; set; }
    }
}
