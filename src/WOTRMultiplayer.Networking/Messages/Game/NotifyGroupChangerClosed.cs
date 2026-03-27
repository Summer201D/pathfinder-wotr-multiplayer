using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGroupChangerClosed)]
    public class NotifyGroupChangerClosed
    {
    }
}
