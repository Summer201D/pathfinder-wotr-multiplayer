using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyKingdomSettlementUpgraded)]
    public class NotifyKingdomSettlementUpgraded
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkKingdomSettlement Settlement { get; set; }
    }
}
