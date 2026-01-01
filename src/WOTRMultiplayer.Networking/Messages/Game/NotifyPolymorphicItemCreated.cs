using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyPolymorphicItemCreated)]
    public class NotifyPolymorphicItemCreated
    {
        [ProtoMember(1)]
        public NetworkPolymorphicItem PolymorphicItem { get; set; }
    }
}
