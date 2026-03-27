using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapCrusadeArmyDismissed)]
    public class NotifyGlobalMapCrusadeArmyDismissed
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkGlobalMapArmy Army { get; set; }
    }
}
