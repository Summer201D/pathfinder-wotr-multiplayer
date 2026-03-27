using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifySkipTimeClosed)]
    public class NotifySkipTimeClosed
    {
    }
}
