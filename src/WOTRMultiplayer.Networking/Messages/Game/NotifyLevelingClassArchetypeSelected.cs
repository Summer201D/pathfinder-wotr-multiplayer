using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLevelingClassArchetypeSelected)]
    public class NotifyLevelingClassArchetypeSelected
    {
        [ProtoMember(1)]
        public NetworkLevelingArchetype Archetype { get; set; }
    }
}
