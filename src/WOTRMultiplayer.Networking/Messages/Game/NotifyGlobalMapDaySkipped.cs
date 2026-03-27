using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapDaySkipped)]
    public class NotifyGlobalMapDaySkipped
    {
    }
}
