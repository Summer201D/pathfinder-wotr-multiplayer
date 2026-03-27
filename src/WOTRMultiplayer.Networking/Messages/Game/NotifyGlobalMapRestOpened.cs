using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapRestOpened)]
    public class NotifyGlobalMapRestOpened
    {
    }
}
