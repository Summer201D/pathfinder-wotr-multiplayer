using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1052)]
    public class NotifyLevelingPhaseChanged
    {
        [ProtoMember(1)]
        public NetworkLevelingPhase Phase { get; set; }
    }
}
