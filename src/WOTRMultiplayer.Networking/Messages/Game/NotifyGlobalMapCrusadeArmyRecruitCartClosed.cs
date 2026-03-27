using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapCrusadeArmyRecruitCartClosed)]
    public class NotifyGlobalMapCrusadeArmyRecruitCartClosed
    {
    }
}
