using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapCrusadeArmySetLeaderClearClicked)]
    public class NotifyGlobalMapCrusadeArmySetLeaderClearClicked
    {
    }
}
