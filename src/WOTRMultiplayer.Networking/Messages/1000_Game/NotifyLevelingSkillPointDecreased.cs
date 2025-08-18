using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1053)]
    public class NotifyLevelingSkillPointDecreased
    {
        [ProtoMember(1)]
        public NetworkLevelingSkillPoint Skill { get; set; }
    }
}
