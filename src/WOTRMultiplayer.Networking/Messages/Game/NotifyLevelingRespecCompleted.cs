using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyLevelingRespecCompleted)]
    public class NotifyLevelingRespecCompleted : IForwardableMessage
    {
    }
}
