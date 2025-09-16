using System.Collections.Generic;
using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkUnitAttack
    {
        [ProtoMember(1)]
        public string ExecutorUnitId { get; set; }

        [ProtoMember(2)]
        public string TargetUnitId { get; set; }

        [ProtoMember(3)]
        public bool IsFullAttack { get; set; }

        [ProtoMember(4)]
        public List<NetworkVector3> VectorPath { get; set; } = [];
    }
}
