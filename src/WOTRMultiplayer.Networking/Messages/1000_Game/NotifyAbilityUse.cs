using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1006)]
    public class NotifyAbilityUse
    {
        [ProtoMember(1)]
        public NetworkAbility Ability { get; set; }
    }
}
