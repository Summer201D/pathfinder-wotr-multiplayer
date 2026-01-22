using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapCrusadeArmyBuyLeaderClosed)]
    public class NotifyGlobalMapCrusadeArmyBuyLeaderClosed
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
