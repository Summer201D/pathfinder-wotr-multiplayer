using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyKingdomEventStarted)]
    public class NotifyKingdomEventStarted
    {
    }
}
