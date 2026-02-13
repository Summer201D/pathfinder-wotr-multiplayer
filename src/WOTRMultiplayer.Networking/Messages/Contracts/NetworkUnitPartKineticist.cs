using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkUnitPartKineticist
    {
        [ProtoMember(1)]
        public int AcceptedBurn { get; set; }
    }
}
