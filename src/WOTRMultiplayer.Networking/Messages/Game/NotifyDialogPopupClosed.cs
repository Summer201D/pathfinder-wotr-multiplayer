using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyDialogPopupClosed)]
    public class NotifyDialogPopupClosed
    {
        [ProtoMember(1)]
        public NetworkDialogPopup Popup { get; set; }
    }
}
