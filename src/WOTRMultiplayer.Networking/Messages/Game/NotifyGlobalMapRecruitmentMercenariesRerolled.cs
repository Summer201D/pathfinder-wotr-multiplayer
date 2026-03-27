using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapRecruitmentMercenariesRerolled)]
    public class NotifyGlobalMapRecruitmentMercenariesRerolled
    {
    }
}
