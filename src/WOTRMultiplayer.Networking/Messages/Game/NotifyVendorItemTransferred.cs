using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyVendorItemTransferred)]
    public class NotifyVendorItemTransferred : IForwardableMessage
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkVendorItemTransfer ItemTransfer { get; set; }
    }
}
