using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyNewGameSequenceLevelingStarted)]
    public class NotifyNewGameSequenceLevelingStarted
    {
    }
}
