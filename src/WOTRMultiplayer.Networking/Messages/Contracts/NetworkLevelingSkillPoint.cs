using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkLevelingSkillPoint
    {
        [ProtoMember(1)]
        public string StatType { get; set; }
    }
}
