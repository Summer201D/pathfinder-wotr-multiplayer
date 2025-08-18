using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1054)]
    public class NotifyLevelingSkillPointIncreased
    {
        [ProtoMember(1)]
        public NetworkLevelingSkillPoint Skill { get; set; }
    }
}
