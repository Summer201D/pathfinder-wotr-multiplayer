using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyLevelingCompleted)]
    public class NotifyLevelingCompleted : IForwardableMessage
    {
    }
}
