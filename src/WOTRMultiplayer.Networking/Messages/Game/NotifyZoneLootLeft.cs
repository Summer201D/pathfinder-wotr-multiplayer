using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyZoneLootLeft)]
    public class NotifyZoneLootLeft
    {
    }
}
