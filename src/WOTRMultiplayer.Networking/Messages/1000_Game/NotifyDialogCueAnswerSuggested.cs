using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(10010)]
    public class NotifyDialogCueAnswerSuggested
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }

        [ProtoMember(2)]
        public string CueName { get; set; }

        [ProtoMember(3)]
        public string DialogName { get; set; }

        [ProtoMember(4)]
        public string AnswerName { get; set; }
    }
}
