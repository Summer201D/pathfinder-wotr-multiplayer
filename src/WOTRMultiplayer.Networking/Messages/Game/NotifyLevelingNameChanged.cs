using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyLevelingNameChanged)]
    public class NotifyLevelingNameChanged : IForwardableMessage
    {
        [ProtoMember(1)]
        [LogMe]
        public string Name { get; set; }
    }
}
