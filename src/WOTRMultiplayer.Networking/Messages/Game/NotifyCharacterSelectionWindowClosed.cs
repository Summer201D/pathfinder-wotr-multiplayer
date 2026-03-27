using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyCharacterSelectionWindowClosed)]
    public class NotifyCharacterSelectionWindowClosed
    {
    }
}
