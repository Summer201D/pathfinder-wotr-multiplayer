using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapCrusadeArmyBuyLeaderShown)]
    public class NotifyGlobalMapCrusadeArmyBuyLeaderShown
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
