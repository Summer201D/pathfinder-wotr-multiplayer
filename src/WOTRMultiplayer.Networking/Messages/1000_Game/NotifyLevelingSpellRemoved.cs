using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1056)]
    public class NotifyLevelingSpellRemoved
    {
        [ProtoMember(1)]
        public NetworkLevelingSpell Spell { get; set; }
    }
}
