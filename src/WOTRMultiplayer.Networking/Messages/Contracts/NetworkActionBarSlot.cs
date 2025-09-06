using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkActionBarSlot
    {
        [ProtoMember(1)]
        public int Index { get; set; }

        [ProtoMember(2)]
        public string UnitId { get; set; }

        [ProtoMember(3)]
        public NetworkItem Item { get; set; }

        [ProtoMember(4)]
        public NetworkAbility Ability { get; set; }

        [ProtoMember(5)]
        public NetworkActivatableAbility ActivatableAbility { get; set; }
    }
}
