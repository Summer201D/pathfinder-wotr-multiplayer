using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapSelectedArmyChanged)]
    public class NotifyGlobalMapSelectedArmyChanged
    {
        [ProtoMember(1)]
        public string ArmyId { get; set; }
    }
}
