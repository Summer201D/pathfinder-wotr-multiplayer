using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLevelingHairColorAppearanceChanged)]
    public class NotifyLevelingHairColorAppearanceChanged
    {
        [ProtoMember(1)]
        public string TextureName { get; set; }
    }
}
