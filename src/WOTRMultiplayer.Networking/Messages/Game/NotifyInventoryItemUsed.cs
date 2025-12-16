using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyInventoryItemUsed)]
    public class NotifyInventoryItemUsed
    {
        [ProtoMember(1)]
        public NetworkUseInventoryItem UseItem { get; set; }
    }
}
