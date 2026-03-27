using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyKingdomExited)]
    public class NotifyKingdomExited : IForwardableMessage
    {
    }
}
