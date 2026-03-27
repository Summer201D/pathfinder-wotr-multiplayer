using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyVendorDealMade)]
    public class NotifyVendorDealMade
    {
    }
}
