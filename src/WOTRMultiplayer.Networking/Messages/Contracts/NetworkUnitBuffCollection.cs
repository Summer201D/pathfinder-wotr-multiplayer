using System.Collections.Generic;
using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkUnitBuffCollection
    {
        [ProtoMember(1)]
        public List<NetworkBuff> Buffs { get; set; } = [];

        [ProtoMember(2)]
        public List<NetworkUnitNegativeLevelsData> NegativeLevels { get; set; } = [];
    }
}
