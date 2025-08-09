using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkCampingState
    {
        [ProtoMember(1)]
        public string CookingBlueprintRecipeId { get; set; }

        [ProtoMember(2)]
        public string PotionBlueprintRecipeId { get; set; }

        [ProtoMember(3)]
        public string ScrollBlueprintRecipeId { get; set; }

        [ProtoMember(4)]
        public bool AutotuneIterationsStatus { get; set; }

        [ProtoMember(5)]
        public int IterationsCount { get; set; }
    }
}
