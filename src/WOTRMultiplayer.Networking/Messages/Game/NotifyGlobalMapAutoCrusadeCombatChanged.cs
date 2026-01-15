using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapAutoCrusadeCombatChanged)]
    public class NotifyGlobalMapAutoCrusadeCombatChanged
    {
        [ProtoMember(1)]
        public bool IsEnabled { get; set; }
    }
}
