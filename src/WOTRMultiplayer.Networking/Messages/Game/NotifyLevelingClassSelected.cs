using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLevelingClassSelected)]
    public class NotifyLevelingClassSelected
    {
        [ProtoMember(1)]
        public NetworkLevelingClass Class { get; set; }
    }
}
