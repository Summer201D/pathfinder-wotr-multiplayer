using ProtoBuf;
using WOTRMultiplayer.Networking.Awaiters;
using WOTRMultiplayer.Networking.Messages.Contracts;

namespace WOTRMultiplayer.Networking.Messages.Requests
{
    [ProtoContract]
    [BeetleX.Packets.MessageType(9002)]
    public class RandomEncounterContextResponse : IAwaitableMessage
    {
        [ProtoMember(1)]
        public NetworkRandomEncounter Context { get; set; }

        public string GetKey()
        {
            return typeof(RandomEncounterContextResponse).Name;
        }
    }
}
