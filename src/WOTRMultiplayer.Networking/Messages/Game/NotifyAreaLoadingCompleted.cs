using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyAreaLoadingCompleted)]
    public class NotifyAreaLoadingCompleted
    {
        [ProtoMember(1)]
        [LogMe]
        public int AreaSeed { get; set; }
    }
}
