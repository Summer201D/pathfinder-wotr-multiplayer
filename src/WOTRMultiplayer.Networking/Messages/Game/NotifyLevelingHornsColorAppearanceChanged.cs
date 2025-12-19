using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLevelingHornsColorAppearanceChanged)]
    public class NotifyLevelingHornsColorAppearanceChanged
    {
        [ProtoMember(1)]
        public string TextureName { get; set; }
    }
}
