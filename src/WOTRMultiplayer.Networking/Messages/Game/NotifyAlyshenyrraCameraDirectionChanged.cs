using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyAlyshenyrraCameraDirectionChanged)]
    public class NotifyAlyshenyrraCameraDirectionChanged
    {
        [ProtoMember(1)]
        [LogMe]
        public string Direction { get; set; }
    }
}
