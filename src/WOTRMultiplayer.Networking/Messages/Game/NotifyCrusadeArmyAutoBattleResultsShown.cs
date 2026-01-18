using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyCrusadeArmyAutoBattleResultsShown)]
    public class NotifyCrusadeArmyAutoBattleResultsShown
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
