using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapCrusadeArmyLeaderLevelingSkillSelected)]
    public class NotifyGlobalMapCrusadeArmyLeaderLevelingSkillSelected
    {
        [ProtoMember(1)]
        public string Id { get; set; }
    }
}
