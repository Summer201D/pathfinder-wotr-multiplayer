using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapCrusadeArmyInfoMergeNameChanged)]
    public class NotifyGlobalMapCrusadeArmyInfoMergeNameChanged
    {
        [ProtoMember(1)]
        public NetworkGlobalMapArmy Army { get; set; }
    }
}
