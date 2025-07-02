using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1005)]
    public class NotifyPartyLeaveArea
    {
        [ProtoMember(1)]
        public string AreaExitId { get; set; }
    }
}
