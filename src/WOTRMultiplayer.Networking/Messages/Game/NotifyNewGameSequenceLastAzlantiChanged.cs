using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyNewGameSequenceLastAzlantiChanged)]
    public class NotifyNewGameSequenceLastAzlantiChanged
    {
        [ProtoMember(1)]
        [LogMe]
        public bool IsEnabled { get; set; }
    }
}
