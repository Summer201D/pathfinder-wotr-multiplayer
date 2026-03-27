using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyLevelingHairStyleAppearanceChanged)]
    public class NotifyLevelingHairStyleAppearanceChanged : IForwardableMessage
    {
        [ProtoMember(1)]
        [LogMe]
        public int Index { get; set; }
    }
}
