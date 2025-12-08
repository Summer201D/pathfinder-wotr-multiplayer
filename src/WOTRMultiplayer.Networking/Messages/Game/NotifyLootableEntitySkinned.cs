using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLootableEntitySkinned)]
    public class NotifyLootableEntitySkinned
    {
        [ProtoMember(1)]
        public NetworkLootableEntity Entity { get; set; }
    }
}
