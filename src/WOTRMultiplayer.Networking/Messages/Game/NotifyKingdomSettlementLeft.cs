using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyKingdomSettlementLeft)]
    public class NotifyKingdomSettlementLeft
    {
    }
}
