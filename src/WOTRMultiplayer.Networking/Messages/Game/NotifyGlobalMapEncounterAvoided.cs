using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Game
{
    [ProtoContract]
    [MessageType((int)MessageTypes.Game.NotifyGlobalMapEncounterAvoided)]
    public class NotifyGlobalMapEncounterAvoided
    {
    }
}
