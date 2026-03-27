using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyLevelingRespecMythicLevelUp)]
    public class NotifyLevelingRespecMythicLevelUp : IForwardableMessage
    {
        [ProtoMember(1)]
        [LogMe]
        public long PlayerId { get; set; }
    }
}
