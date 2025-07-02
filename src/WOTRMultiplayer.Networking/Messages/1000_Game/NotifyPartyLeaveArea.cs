using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1005)]
    public class NotifyPartyLeaveArea
    {
    }
}
