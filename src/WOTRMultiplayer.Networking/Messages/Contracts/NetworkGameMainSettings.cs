using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkGameMainSettings
    {
        [ProtoMember(1)]
        public bool LootInCombat { get; set; }

        [ProtoMember(2)]
        public bool QuickMovement { get; set; }
    }
}
