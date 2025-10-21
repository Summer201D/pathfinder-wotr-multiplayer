using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapIngredientCollectionAccepted)]
    public class NotifyGlobalMapIngredientCollectionAccepted
    {
        [ProtoMember(1)]
        public NetworkGlobalMapLocation Location { get; set; }
    }
}
