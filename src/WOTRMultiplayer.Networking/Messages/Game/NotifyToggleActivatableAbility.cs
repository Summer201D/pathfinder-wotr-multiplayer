using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyToggleActivatableAbility)]
    public class NotifyToggleActivatableAbility : IForwardableMessage
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkActivatableAbility Ability { get; set; }
    }
}
