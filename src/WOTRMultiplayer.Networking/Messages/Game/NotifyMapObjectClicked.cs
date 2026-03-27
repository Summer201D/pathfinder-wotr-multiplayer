using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyMapObjectClicked)]
    public class NotifyMapObjectClicked : IForwardableMessage
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkClick Click { get; set; }
    }
}
