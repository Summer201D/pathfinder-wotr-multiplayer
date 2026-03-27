using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapRecruitmentNextArmySelected)]
    public class NotifyGlobalMapRecruitmentNextArmySelected
    {
    }
}
