using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyDialogStarted)]
    public class NotifyDialogStarted
    {
        [ProtoMember(1)]
        public NetworkDialog Dialog { get; set; }
    }
}
