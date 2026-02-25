using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkIslandMapTransition
    {
        [ProtoMember(1)]
        [LogMe]
        public string BlueprintId { get; set; }

        [ProtoMember(2)]
        [LogMe]
        public NetworkVector2Int Position { get; set; }
    }
}
