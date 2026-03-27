using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyTransitionMapClosed)]
    public class NotifyTransitionMapClosed
    {
    }
}
