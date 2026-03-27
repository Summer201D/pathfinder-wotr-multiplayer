using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapCrusadeArmyMainCartClosed)]
    public class NotifyGlobalMapCrusadeArmyMainCartClosed
    {
    }
}
