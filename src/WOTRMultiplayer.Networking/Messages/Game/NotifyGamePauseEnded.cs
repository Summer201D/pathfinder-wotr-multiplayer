using System.Collections.Generic;
using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyGamePauseEnded)]
    public class NotifyGamePauseEnded
    {
        [ProtoMember(1)]
        public int? AreaSeed { get; set; }

        [ProtoMember(2)]
        public List<NetworkUnit> Party { get; set; } = [];
    }
}
