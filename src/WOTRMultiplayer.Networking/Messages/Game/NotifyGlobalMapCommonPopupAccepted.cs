using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapCommonPopupAccepted)]
    public class NotifyGlobalMapCommonPopupAccepted
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkGlobalMapCommonPopup Popup { get; set; }
    }
}
