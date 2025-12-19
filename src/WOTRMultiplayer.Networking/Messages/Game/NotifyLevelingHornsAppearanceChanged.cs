using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLevelingHornsAppearanceChanged)]
    public class NotifyLevelingHornsAppearanceChanged
    {
        [ProtoMember(1)]
        public int Index { get; set; }
    }
}
