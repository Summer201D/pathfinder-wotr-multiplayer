using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkNewGameSequencePhase
    {
        [ProtoMember(1)]
        public string Type { get; set; }
    }
}
