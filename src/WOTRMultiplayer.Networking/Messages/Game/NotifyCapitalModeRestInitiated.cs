using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyCapitalModeRestInitiated)]
    public class NotifyCapitalModeRestInitiated : IForwardableMessage
    {
    }
}
