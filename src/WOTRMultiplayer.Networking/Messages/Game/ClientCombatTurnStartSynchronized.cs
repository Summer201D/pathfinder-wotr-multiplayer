using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.ClientCombatTurnStartSynchronized)]
    public class ClientCombatTurnStartSynchronized
    {
        [ProtoMember(1)]
        [LogMe]
        public string UnitId { get; set; }
    }
}
