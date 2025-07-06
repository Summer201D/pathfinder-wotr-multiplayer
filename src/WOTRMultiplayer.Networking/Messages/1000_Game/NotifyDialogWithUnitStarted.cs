using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1013)]
    public class NotifyDialogWithUnitStarted
    {
        [ProtoMember(1)]
        public string DialogName { get; set; }

        [ProtoMember(2)]
        public string TargetUnitId { get; set; }

        [ProtoMember(3)]
        public string InitiatorUnitId { get; set; }
    }
}
