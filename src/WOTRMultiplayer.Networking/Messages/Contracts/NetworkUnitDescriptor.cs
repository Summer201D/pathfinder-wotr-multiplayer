using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkUnitDescriptor
    {
        [ProtoMember(1)]
        public int Damage { get; set; }

        [ProtoMember(2)]
        public NetworkCharacterStats Stats { get; set; }

        [ProtoMember(3)]
        public NetworkUnitState State { get; set; }
    }
}
