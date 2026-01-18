using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapCommonPopupAccepted)]
    public class NotifyGlobalMapCommonPopupAccepted
    {
        [ProtoMember(1)]
        public NetworkGlobalMapCommonPopup Popup { get; set; }
    }
}
