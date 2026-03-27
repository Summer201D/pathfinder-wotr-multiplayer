using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifySkipTimeStarted)]
    public class NotifySkipTimeStarted
    {
    }
}
