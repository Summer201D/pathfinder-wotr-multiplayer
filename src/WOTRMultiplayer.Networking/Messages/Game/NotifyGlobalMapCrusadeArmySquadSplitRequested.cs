using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapCrusadeArmySquadSplitRequested)]
    public class NotifyGlobalMapCrusadeArmySquadSplitRequested
    {
        [ProtoMember(1)]
        public NetworkGlobalMapArmySquadSlot SourceSquadSlot { get; set; }

        [ProtoMember(2)]
        public NetworkGlobalMapArmySquadSlot TargetSquadSlot { get; set; }

        [ProtoMember(3)]
        public int Count { get; set; }
    }
}
