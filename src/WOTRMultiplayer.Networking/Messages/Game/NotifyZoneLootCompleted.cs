using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyZoneLootCompleted)]
    public class NotifyZoneLootCompleted
    {
    }
}
