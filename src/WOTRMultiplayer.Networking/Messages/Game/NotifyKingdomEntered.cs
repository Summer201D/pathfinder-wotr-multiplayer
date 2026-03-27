using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyKingdomEntered)]
    public class NotifyKingdomEntered : IForwardableMessage
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkKingdomEntryPoint EntryPoint { get; set; }
    }
}
