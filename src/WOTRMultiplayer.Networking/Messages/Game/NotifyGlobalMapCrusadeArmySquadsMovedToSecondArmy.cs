using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapCrusadeArmySquadsMovedToSecondArmy)]
    public class NotifyGlobalMapCrusadeArmySquadsMovedToSecondArmy
    {
    }
}
