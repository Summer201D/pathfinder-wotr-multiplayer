using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyDungeonBoonSelected)]
    public class NotifyDungeonBoonSelected
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkBoon Boon { get; set; }
    }
}
