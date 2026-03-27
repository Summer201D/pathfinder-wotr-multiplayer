using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapCrusadeArmiesMerging)]
    public class NotifyGlobalMapCrusadeArmiesMerging
    {
    }
}
