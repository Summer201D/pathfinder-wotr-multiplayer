using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLevelingPrimaryOutfitColorAppearanceChanged)]
    public class NotifyLevelingPrimaryOutfitColorAppearanceChanged
    {
        [ProtoMember(1)]
        public string TextureName { get; set; }
    }
}
