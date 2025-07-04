using System.Collections.Generic;
using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages
{
    [ProtoContract]
    public class RollDice
    {
        [ProtoMember(1)]
        public int Result { get; set; }

        [ProtoMember(2)]
        public List<int> RollHistory { get; set; } = [];
    }
}
