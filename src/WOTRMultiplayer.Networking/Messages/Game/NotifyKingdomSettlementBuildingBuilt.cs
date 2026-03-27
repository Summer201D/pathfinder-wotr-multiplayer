using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyKingdomSettlementBuildingBuilt)]
    public class NotifyKingdomSettlementBuildingBuilt
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkKingdomSettlementBuilding Building { get; set; }
    }
}
