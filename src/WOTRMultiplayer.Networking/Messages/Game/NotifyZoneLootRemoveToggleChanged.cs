using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyZoneLootRemoveToggleChanged)]
    public class NotifyZoneLootRemoveToggleChanged
    {
        [ProtoMember(1)]
        public bool RemoveLoot { get; set; }
    }
}
