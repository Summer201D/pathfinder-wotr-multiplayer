using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLevelingBodyTypeAppearanceChanged)]
    public class NotifyLevelingBodyTypeAppearanceChanged
    {
        [ProtoMember(1)]
        public int Index { get; set; }
    }
}
