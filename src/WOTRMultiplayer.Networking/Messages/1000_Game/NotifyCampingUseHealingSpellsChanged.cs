using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1034)]
    public class NotifyCampingUseHealingSpellsChanged
    {
        [ProtoMember(1)]
        public bool IsOn { get; set; }
    }
}
