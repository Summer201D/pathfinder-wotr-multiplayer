using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyDungeonGameOverNewGameStarted)]
    public class NotifyDungeonGameOverNewGameStarted
    {
    }
}
