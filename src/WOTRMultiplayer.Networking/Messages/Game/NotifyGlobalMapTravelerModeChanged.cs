using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapTravelerModeChanged)]
    public class NotifyGlobalMapTravelerModeChanged
    {
        [ProtoMember(1)]
        public string TravelerMode { get; set; }
    }
}
