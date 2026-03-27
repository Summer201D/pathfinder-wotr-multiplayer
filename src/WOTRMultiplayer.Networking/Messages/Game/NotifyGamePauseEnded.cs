using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGamePauseEnded)]
    public class NotifyGamePauseEnded
    {
    }
}
