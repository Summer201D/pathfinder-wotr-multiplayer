using System;
using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkForcedPause
    {
        [ProtoMember(1)]
        public string Reason { get; set; }

        [ProtoMember(2)]
        public TimeSpan? RemovalDelay { get; set; }
    }
}
