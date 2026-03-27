using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapCrusadeArmyCreated)]
    public class NotifyGlobalMapCrusadeArmyCreated
    {
    }
}
