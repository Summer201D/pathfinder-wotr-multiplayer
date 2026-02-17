using System.Collections.Generic;
using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkUnitMoveTo
    {
        [ProtoMember(1)]
        public string InitiatorUnitId { get; set; }

        [ProtoMember(2)]
        public List<NetworkVector3> VectorPath { get; set; } = [];

        [ProtoMember(3)]
        public NetworkVector3 Destination { get; set; }

        [ProtoMember(4)]
        public string MovementLimit { get; set; }
    }
}
