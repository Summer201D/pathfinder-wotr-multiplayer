using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyActionBarSlotCleared)]
    public class NotifyActionBarSlotCleared
    {
        [ProtoMember(1)]
        public NetworkActionBarSlot ActionBarSlot { get; set; }
    }
}
