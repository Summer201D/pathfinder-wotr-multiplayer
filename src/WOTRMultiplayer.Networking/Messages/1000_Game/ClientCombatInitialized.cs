using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(10013)]
    public class ClientCombatInitialized
    {
    }
}
