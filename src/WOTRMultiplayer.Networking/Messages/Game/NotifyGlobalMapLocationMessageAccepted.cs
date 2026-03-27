using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapLocationMessageAccepted)]
    public class NotifyGlobalMapLocationMessageAccepted
    {
    }
}
