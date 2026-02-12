using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkUnitPartInPit
    {
        [ProtoMember(1)]
        public float CurrentRoundSeconds { get; set; }

        [ProtoMember(2)]
        public string State { get; set; }
    }
}
