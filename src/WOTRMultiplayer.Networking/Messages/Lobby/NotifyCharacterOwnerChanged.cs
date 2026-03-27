using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Lobby
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Lobby.NotifyCharacterOwnerChanged)]
    public class NotifyCharacterOwnerChanged
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkCharacter Character { get; set; }
    }
}
