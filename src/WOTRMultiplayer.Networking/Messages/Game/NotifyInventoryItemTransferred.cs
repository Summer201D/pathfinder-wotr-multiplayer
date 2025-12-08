using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyInventoryItemTransferred)]
    public class NotifyInventoryItemTransferred
    {
        [ProtoMember(1)]
        public NetworkItemsTransfer TransferItem { get; set; }
    }
}
