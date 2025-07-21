using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(10020)]
    public class NotifyAbilityClicked
    {
        [ProtoMember(1)]
        public NetworkClick Click { get; set; }
    }
}
