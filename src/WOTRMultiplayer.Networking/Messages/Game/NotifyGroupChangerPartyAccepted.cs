using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGroupChangerPartyAccepted)]
    public class NotifyGroupChangerPartyAccepted
    {
    }
}
