using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapUnitsRecruited)]
    public class NotifyGlobalMapUnitsRecruited
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkGlobalMapUnitRecruitmentOrder Order { get; set; }
    }
}
