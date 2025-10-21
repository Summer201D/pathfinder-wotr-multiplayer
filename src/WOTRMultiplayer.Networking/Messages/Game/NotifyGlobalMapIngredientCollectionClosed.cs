using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapIngredientCollectionClosed)]
    public class NotifyGlobalMapIngredientCollectionClosed
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
