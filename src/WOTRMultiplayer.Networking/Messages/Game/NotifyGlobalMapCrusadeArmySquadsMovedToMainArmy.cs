using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapCrusadeArmySquadsMovedToMainArmy)]
    public class NotifyGlobalMapCrusadeArmySquadsMovedToMainArmy
    {
    }
}
