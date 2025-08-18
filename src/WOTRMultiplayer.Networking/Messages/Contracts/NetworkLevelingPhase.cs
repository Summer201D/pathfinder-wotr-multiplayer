using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkLevelingPhase
    {
        [ProtoMember(1)]
        public int Index { get; set; }
    }
}
