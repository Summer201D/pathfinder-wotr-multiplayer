using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapCrusadeArmyLeaderLevelingClosed)]
    public class NotifyGlobalMapCrusadeArmyLeaderLevelingClosed
    {
    }
}
