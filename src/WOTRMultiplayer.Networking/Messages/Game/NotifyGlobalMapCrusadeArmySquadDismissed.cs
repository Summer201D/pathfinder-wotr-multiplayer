using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGlobalMapCrusadeArmySquadDismissed)]
    public class NotifyGlobalMapCrusadeArmySquadDismissed
    {
        [ProtoMember(1)]
        public NetworkGlobalMapArmySquadSlot SquadSlot { get; set; }
    }
}
