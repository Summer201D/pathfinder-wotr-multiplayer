using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyCampingUseHealingSpellsChanged)]
    public class NotifyCampingUseHealingSpellsChanged
    {
        [ProtoMember(1)]
        [LogMe]
        public bool IsOn { get; set; }
    }
}
