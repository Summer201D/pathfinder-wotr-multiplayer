using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLevelingHairStyleAppearanceChanged)]
    public class NotifyLevelingHairStyleAppearanceChanged
    {
        [ProtoMember(1)]
        public int Index { get; set; }
    }
}
