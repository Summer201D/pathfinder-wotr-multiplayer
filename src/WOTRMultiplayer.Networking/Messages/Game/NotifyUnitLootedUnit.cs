using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyUnitLootedUnit)]
    public class NotifyUnitLootedUnit : IForwardableMessage
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkUnitLootUnit LootUnit { get; set; }
    }
}
