using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkGlobalMapCommonPopup
    {
        [ProtoMember(1)]
        public NetworkGlobalMapLocation Location { get; set; }

        [ProtoMember(2)]
        public string Type { get; set; }
    }
}
