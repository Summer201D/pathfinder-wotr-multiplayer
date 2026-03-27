using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapCrusadeArmyInfoNextMergeArmySelected)]
    public class NotifyGlobalMapCrusadeArmyInfoNextMergeArmySelected
    {
    }
}
