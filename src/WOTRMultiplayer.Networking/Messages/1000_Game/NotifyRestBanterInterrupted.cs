using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1041)]
    public class NotifyRestBanterInterrupted
    {
        [ProtoMember(1)]
        public NetworkRestBanter Banter { get; set; }
    }
}
