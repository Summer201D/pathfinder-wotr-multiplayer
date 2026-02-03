using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLevelingStarted)]
    public class NotifyLevelingStarted
    {
        [ProtoMember(1)]
        public string UnitId { get; set; }

        [ProtoMember(2)]
        public string Type { get; set; }
    }
}
