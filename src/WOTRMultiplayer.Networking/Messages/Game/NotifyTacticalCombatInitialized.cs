using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyTacticalCombatInitialized)]
    public class NotifyTacticalCombatInitialized
    {
        [ProtoMember(1)]
        public int Seed { get; set; }
    }
}
