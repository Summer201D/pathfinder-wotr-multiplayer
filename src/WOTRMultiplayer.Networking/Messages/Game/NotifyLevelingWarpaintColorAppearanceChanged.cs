using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyLevelingWarpaintColorAppearanceChanged)]
    public class NotifyLevelingWarpaintColorAppearanceChanged
    {
        [ProtoMember(1)]
        public NetworkLevelingWarpaint Warpaint { get; set; }
    }
}
