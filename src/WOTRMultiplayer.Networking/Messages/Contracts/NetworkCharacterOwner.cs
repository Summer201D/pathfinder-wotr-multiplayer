using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkCharacterOwner
    {
        [ProtoMember(1)]
        public int CharacterIndex { get; set; }

        [ProtoMember(2)]
        public long PlayerId { get; set; }
    }
}
