using System;
using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkUnitNegativeLevelsData
    {
        [ProtoMember(1)]
        public int Count { get; set; }

        [ProtoMember(2)]
        public string SavingThrowType { get; set; }

        [ProtoMember(3)]
        public string EnergyDrainType { get; set; }

        [ProtoMember(4)]
        public TimeSpan? Duration { get; set; }
    }
}
