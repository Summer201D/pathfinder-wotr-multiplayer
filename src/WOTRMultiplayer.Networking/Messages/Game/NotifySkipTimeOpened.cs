using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifySkipTimeOpened)]
    public class NotifySkipTimeOpened
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
