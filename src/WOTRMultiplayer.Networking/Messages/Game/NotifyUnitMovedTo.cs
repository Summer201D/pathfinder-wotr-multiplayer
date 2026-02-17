using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyUnitMovedTo)]
    public class NotifyUnitMovedTo
    {
        [ProtoMember(1)]
        public NetworkUnitMoveTo Movement { get; set; }
    }
}
