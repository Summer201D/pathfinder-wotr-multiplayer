using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkAbilityParams
    {
        [ProtoMember(1)]
        public int CasterLevel { get; set; }

        [ProtoMember(2)]
        public int Concentration { get; set; }

        [ProtoMember(3)]
        public int DC { get; set; }

        [ProtoMember(4)]
        public string SpellSource { get; set; }

        [ProtoMember(5)]
        public int Metamagic { get; set; }

        [ProtoMember(6)]
        public int RankBonus { get; set; }
    }
}
