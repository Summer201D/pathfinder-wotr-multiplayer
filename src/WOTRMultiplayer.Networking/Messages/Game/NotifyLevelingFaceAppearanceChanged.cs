using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLevelingFaceAppearanceChanged)]
    public class NotifyLevelingFaceAppearanceChanged
    {
        [ProtoMember(1)]
        public int Index { get; set; }
    }
}
