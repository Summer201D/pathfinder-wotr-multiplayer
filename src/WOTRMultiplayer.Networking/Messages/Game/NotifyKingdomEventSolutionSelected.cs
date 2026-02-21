using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyKingdomEventSolutionSelected)]
    public class NotifyKingdomEventSolutionSelected
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkKingdomEventSolution Solution { get; set; }
    }
}
