using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkUnitState
    {
        [ProtoMember(1)]
        public bool IsCharging { get; set; }
    }
}
