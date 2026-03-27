using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapCrusadeArmySetLeaderRecruitClicked)]
    public class NotifyGlobalMapCrusadeArmySetLeaderRecruitClicked
    {
    }
}
