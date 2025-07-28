using System;
using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1006)]
    public class DiceRollValueRequest
    {
        [ProtoMember(1)]
        public int RollId { get; set; }

        [ProtoMember(2)]
        public TimeSpan Timeout { get; set; }
    }
}
