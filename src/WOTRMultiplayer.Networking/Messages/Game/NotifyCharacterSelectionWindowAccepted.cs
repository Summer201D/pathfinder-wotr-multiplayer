using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyCharacterSelectionWindowAccepted)]
    public class NotifyCharacterSelectionWindowAccepted
    {
    }
}
