using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkGlobalMapArmy
    {
        [ProtoMember(1)]
        public string Id { get; set; }
    }
}
