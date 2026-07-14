using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyDungeonBoonConfirmed)]
    public class NotifyDungeonBoonConfirmed
    {
    }
}
