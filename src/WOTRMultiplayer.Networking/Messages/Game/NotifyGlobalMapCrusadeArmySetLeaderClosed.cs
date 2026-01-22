using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapCrusadeArmySetLeaderClosed)]
    public class NotifyGlobalMapCrusadeArmySetLeaderClosed
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
