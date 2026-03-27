using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.ClientAreaLoaded)]
    public class ClientAreaLoaded
    {
    }
}
