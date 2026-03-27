using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyCombatInitializationCompleted)]
    public class NotifyCombatInitializationCompleted
    {
    }
}
