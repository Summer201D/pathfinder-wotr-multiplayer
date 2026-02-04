using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkUnitDescriptor
    {
        [ProtoMember(1)]
        public int Damage { get; set; }
    }
}
