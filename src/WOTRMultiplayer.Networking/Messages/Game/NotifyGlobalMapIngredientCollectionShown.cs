using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapIngredientCollectionShown)]
    public class NotifyGlobalMapIngredientCollectionShown
    {
        [ProtoMember(1)]
        public long PlayerId { get; set; }
    }
}
