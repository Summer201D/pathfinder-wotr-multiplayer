using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkKingdomEventSolution
    {
        [ProtoMember(1)]
        [LogMe]
        public int Index { get; set; }

        [ProtoMember(2)]
        [LogMe]
        public string Name { get; set; }
    }
}
