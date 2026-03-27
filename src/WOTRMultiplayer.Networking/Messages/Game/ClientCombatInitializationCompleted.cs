using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.ClientCombatInitializationCompleted)]
    public class ClientCombatInitializationCompleted
    {
    }
}
