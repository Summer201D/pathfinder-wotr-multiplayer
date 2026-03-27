using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyLevelingTerminated)]
    public class NotifyLevelingTerminated : IForwardableMessage
    {
    }
}
