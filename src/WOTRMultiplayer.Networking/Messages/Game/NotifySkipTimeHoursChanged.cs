using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifySkipTimeHoursChanged)]
    public class NotifySkipTimeHoursChanged
    {
        [ProtoMember(1)]
        public float Hours { get; set; }
    }
}
