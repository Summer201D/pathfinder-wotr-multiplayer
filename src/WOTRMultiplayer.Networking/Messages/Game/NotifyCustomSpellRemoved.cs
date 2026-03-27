using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyCustomSpellRemoved)]
    public class NotifyCustomSpellRemoved : IForwardableMessage
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkAbility Ability { get; set; }

        [ProtoMember(2)]
        [LogMe]
        public string UnitId { get; set; }
    }
}
