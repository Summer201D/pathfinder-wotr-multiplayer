using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLevelingGenderSelected)]
    public class NotifyLevelingGenderSelected
    {
        [ProtoMember(1)]
        public string GenderId { get; set; }
    }
}
