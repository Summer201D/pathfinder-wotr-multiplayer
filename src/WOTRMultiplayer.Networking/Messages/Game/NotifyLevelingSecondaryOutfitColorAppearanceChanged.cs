using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLevelingSecondaryOutfitColorAppearanceChanged)]
    public class NotifyLevelingSecondaryOutfitColorAppearanceChanged
    {
        [ProtoMember(1)]
        public string TextureName { get; set; }
    }
}
