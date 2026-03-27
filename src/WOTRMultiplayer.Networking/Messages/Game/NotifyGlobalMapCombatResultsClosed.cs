using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapCombatResultsClosed)]
    public class NotifyGlobalMapCombatResultsClosed
    {
    }
}
