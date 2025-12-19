using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkLevelingTattoo
    {
        [ProtoMember(1)]
        public int Index { get; set; }

        [ProtoMember(2)]
        public int PageNumber { get; set; }

        [ProtoMember(3)]
        public string TextureName { get; set; }
    }
}
