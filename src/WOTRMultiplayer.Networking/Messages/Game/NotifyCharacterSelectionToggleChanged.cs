using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyCharacterSelectionToggleChanged)]
    public class NotifyCharacterSelectionToggleChanged
    {
        [ProtoMember(1)]
        public string UnitId { get; set; }
    }
}
