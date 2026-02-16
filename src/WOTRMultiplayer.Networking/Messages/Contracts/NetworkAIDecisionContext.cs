using System.Collections.Generic;
using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkAIDecisionContext
    {
        [ProtoMember(1)]
        public List<NetworkVector3> VectorPath { get; set; } = [];

        [ProtoMember(2)]
        public bool BestEnableFiveFootStep { get; set; }

        [ProtoMember(3)]
        public NetworkVector3 BestDestinationPoint { get; set; }
    }
}
