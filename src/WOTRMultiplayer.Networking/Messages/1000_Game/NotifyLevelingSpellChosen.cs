using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1057)]
    public class NotifyLevelingSpellChosen
    {
        [ProtoMember(1)]
        public NetworkLevelingSpell Spell { get; set; }
    }
}
