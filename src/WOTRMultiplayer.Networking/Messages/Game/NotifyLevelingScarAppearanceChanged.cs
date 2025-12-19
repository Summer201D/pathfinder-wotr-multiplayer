using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLevelingScarAppearanceChanged)]
    public class NotifyLevelingScarAppearanceChanged
    {
        [ProtoMember(1)]
        public int Index { get; set; }
    }
}
