using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyActionBarSlotMoved)]
    public class NotifyActionBarSlotMoved
    {
        [ProtoMember(1)]
        public NetworkActionBarSlot SourceActionBarSlot { get; set; }

        [ProtoMember(2)]
        public NetworkActionBarSlot TargetActionBarSlot { get; set; }
    }
}
