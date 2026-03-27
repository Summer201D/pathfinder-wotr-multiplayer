using System.Collections.Generic;
using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Lobby
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Lobby.NotifySaveGameTransferProgressChanged)]
    [ExcludeFromLogging]
    public class NotifySaveGameTransferProgressChanged
    {
        [ProtoMember(1)]
        public Dictionary<long, float> Progress { get; set; } = [];
    }
}
