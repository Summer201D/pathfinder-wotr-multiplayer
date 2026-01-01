using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyPolymorphicItemCreationRequested)]
    public class NotifyPolymorphicItemCreationRequested
    {
        [ProtoMember(1)]
        public NetworkPolymorphicItem PolymorphicItem { get; set; }
    }
}
