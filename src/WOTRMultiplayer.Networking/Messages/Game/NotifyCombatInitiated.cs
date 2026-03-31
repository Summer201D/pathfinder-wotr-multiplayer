using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyCombatInitiated)]
    public class NotifyCombatInitiated
    {
        [ProtoMember(1)]
        [LogMe]
        public long PlayerId { get; set; }
    }
}
