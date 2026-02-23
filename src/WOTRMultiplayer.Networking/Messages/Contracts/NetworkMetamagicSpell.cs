using System.Collections.Generic;
using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkMetamagicSpell
    {
        [ProtoMember(1)]
        [LogMe]
        public string UnitId { get; set; }

        [ProtoMember(2)]
        [LogMe]
        public NetworkAbility Ability { get; set; }

        [ProtoMember(3)]
        [LogMe]
        public List<int> MetamagicFeatures { get; set; } = [];

        [ProtoMember(4)]
        [LogMe]
        public int? BorderNumber { get; set; }

        [ProtoMember(5)]
        [LogMe]
        public int? DecorationColorNumber { get; set; }

        [ProtoMember(6)]
        [LogMe]
        public int HeightenLevel { get; set; }
    }
}
