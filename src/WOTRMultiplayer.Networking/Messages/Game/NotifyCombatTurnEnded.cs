using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyCombatTurnEnded)]
    public class NotifyCombatTurnEnded
    {
    }
}
