using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1031)]
    public class NotifyPerceptionCheckRolled
    {
        [ProtoMember(1)]
        public NetworkPerceptionCheck Check { get; set; }
    }
}
