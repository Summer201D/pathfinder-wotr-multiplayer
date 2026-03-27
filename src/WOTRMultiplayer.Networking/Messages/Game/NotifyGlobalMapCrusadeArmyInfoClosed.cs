using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapCrusadeArmyInfoClosed)]
    public class NotifyGlobalMapCrusadeArmyInfoClosed
    {
    }
}
