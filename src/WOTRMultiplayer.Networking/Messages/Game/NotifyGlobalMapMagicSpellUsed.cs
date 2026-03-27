using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapMagicSpellUsed)]
    public class NotifyGlobalMapMagicSpellUsed
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkGlobalMapMagicSpell Spell { get; set; }
    }
}
