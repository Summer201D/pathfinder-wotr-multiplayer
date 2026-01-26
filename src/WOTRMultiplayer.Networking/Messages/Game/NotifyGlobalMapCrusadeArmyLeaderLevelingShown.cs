using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapCrusadeArmyLeaderLevelingShown)]
    public class NotifyGlobalMapCrusadeArmyLeaderLevelingShown
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
