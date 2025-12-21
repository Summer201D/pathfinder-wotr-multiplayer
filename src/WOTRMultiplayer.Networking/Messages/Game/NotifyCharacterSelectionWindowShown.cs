using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyCharacterSelectionWindowShown)]
    public class NotifyCharacterSelectionWindowShown
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
