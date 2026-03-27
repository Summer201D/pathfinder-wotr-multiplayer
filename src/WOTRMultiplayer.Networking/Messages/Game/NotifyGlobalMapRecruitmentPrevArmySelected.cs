using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapRecruitmentPrevArmySelected)]
    public class NotifyGlobalMapRecruitmentPrevArmySelected
    {
    }
}
