using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyPolymorphicItemCreationRequested)]
    public class NotifyPolymorphicItemCreationRequested
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkPolymorphicItem PolymorphicItem { get; set; }
    }
}
