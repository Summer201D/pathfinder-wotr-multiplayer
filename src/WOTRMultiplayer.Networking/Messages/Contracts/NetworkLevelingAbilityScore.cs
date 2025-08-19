using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkLevelingAbilityScore
    {
        [ProtoMember(1)]
        public string StatType { get; set; }
    }
}
