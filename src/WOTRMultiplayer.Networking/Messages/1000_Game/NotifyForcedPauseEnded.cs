using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(1003)]
    public class NotifyForcedPauseEnded
    {
    }
}
