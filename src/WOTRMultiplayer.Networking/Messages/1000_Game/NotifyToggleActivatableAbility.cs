using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1024)]
    public class NotifyToggleActivatableAbility
    {
        [ProtoMember(1)]
        public NetworkActivatableAbility Ability { get; set; }
    }
}
