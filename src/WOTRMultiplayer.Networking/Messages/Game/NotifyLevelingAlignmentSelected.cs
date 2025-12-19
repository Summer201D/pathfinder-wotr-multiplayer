using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLevelingAlignmentSelected)]
    public class NotifyLevelingAlignmentSelected
    {
        [ProtoMember(1)]
        public string AlignmentId { get; set; }
    }
}
