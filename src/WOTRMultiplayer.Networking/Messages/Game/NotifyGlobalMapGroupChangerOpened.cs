using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapGroupChangerOpened)]
    public class NotifyGlobalMapGroupChangerOpened
    {
    }
}
