using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyCrusadeArmyBattleResultsManualCombatStarted)]
    public class NotifyCrusadeArmyBattleResultsManualCombatStarted
    {
    }
}
