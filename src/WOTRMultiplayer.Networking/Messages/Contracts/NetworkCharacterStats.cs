using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkCharacterStats
    {
        [ProtoMember(1)]
        public int DamageNonLethal { get; set; }
    }
}
