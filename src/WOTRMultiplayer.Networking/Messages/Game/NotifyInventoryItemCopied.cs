using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyInventoryItemCopied)]
    public class NotifyInventoryItemCopied : IForwardableMessage
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkItemCopy Copy { get; set; }
    }
}
