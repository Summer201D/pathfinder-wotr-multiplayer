using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGroupChangerUnitClicked)]
    public class NotifyGroupChangerUnitClicked
    {
        [ProtoMember(1)]
        public string UnitId { get; set; }
    }
}
