using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyVendorWindowClosed)]
    public class NotifyVendorWindowClosed
    {
    }
}
