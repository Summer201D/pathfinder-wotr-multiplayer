using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLevelingMythicClassSelected)]
    public class NotifyLevelingMythicClassSelected
    {
        [ProtoMember(1)]
        public string MythicClassId { get; set; }
    }
}
