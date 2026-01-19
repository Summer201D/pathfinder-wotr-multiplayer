using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapCrusadeArmySquadsSwitched)]
    public class NotifyGlobalMapCrusadeArmySquadsSwitched
    {
        [ProtoMember(1)]
        public NetworkGlobalMapArmySquadSlot SourceSquadSlot { get; set; }

        [ProtoMember(2)]
        public NetworkGlobalMapArmySquadSlot TargetSquadSlot { get; set; }
    }
}
