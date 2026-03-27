using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyIslandMapEntryChosen)]
    public class NotifyIslandMapEntryChosen
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkIslandMapTransition Island { get; set; }
    }
}
