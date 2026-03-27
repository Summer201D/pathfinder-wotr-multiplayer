using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyLevelingFaceAppearanceChanged)]
    public class NotifyLevelingFaceAppearanceChanged : IForwardableMessage
    {
        [ProtoMember(1)]
        [LogMe]
        public int Index { get; set; }
    }
}
