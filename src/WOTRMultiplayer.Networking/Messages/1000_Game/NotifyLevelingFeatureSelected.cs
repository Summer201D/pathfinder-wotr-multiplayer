using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1055)]
    public class NotifyLevelingFeatureSelected
    {
        [ProtoMember(1)]
        public NetworkLevelingFeature Feature { get; set; }
    }
}
