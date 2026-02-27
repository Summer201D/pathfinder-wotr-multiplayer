using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkUnitInteractWithUnit
    {
        [ProtoMember(1)]
        [LogMe]
        public string InitiatorUnitId { get; set; }

        [ProtoMember(2)]
        [LogMe]
        public string TargetUnitId { get; set; }
    }
}
