using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyKingdomSettlementBuildingSold)]
    public class NotifyKingdomSettlementBuildingSold
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkKingdomSettlementBuilding Building { get; set; }
    }
}
