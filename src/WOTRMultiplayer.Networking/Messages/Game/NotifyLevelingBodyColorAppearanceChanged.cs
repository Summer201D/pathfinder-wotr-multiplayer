using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLevelingBodyColorAppearanceChanged)]
    public class NotifyLevelingBodyColorAppearanceChanged
    {
        [ProtoMember(1)]
        public string TextureName { get; set; }
    }
}
