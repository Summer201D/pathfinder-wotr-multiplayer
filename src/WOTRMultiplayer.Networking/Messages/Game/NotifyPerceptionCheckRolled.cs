using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyPerceptionCheckRolled)]
    public class NotifyPerceptionCheckRolled
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkPerceptionCheck Check { get; set; }
    }
}
