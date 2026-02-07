using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.ClientGameAutoPaused)]
    public class ClientGameAutoPaused
    {
        [ProtoMember(1)]
        public NetworkForcedPause Pause { get; set; }
    }
}
