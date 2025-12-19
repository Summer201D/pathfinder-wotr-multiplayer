using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLevelingWarpaintAppearanceChanged)]
    public class NotifyLevelingWarpaintAppearanceChanged
    {
        [ProtoMember(1)]
        public NetworkLevelingWarpaint Warpaint { get; set; }
    }
}
