using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyKingdomNavigationChanged)]
    public class NotifyKingdomNavigationChanged
    {
        [ProtoMember(1)]
        [LogMe]
        public string Type { get; set; }

        [ProtoMember(2)]
        [LogMe]
        public long PlayerId { get; set; }
    }
}
