using ProtoBuf;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType((int)MessageTypes.Game.NotifyKingdomEntered)]
    public class NotifyKingdomEntered
    {
        [ProtoMember(1)]
        public NetworkKingdomEntryPoint EntryPoint { get; set; }
    }
}
