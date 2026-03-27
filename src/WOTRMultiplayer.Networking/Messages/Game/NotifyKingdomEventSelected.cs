using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyKingdomEventSelected)]
    public class NotifyKingdomEventSelected
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkKingdomEvent Event { get; set; }
    }
}
