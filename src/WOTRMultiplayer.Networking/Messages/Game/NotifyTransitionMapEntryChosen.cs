using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyTransitionMapEntryChosen)]
    public class NotifyTransitionMapEntryChosen
    {
        [ProtoMember(1)]
        [LogMe]
        public string EntryId { get; set; }
    }
}
