using ProtoBuf;
using WOTRMultiplayer.Logging.Attributes;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyNewGameSequenceCampaignChanged)]
    public class NotifyNewGameSequenceCampaignChanged
    {
        [ProtoMember(1)]
        [LogMe]
        public NetworkCampaign Campaign { get; set; }
    }
}
