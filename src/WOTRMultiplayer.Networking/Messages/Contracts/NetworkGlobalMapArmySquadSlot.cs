using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkGlobalMapArmySquadSlot
    {
        [ProtoMember(1)]
        public string SquadId { get; set; }

        [ProtoMember(2)]
        public string ArmyId { get; set; }

        [ProtoMember(3)]
        public NetworkVector2Int Position { get; set; }
    }
}
