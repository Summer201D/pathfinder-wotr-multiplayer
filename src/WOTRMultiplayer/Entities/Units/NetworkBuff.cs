using System;

namespace WOTRMultiplayer.Entities.Units
{
    public class NetworkBuff
    {
        public string Id { get; set; }

        public string BlueprintId { get; set; }

        public string Name { get; set; }

        public bool IsPermanent { get; set; }

        public TimeSpan TimeLeft { get; set; }

        public TimeSpan NextTickTime { get; set; }

        public TimeSpan NextResourceSpendingTime { get; set; }

        public string CasterId { get; set; }

        public int Rank { get; set; }

        public NetworkAbilityParams AbilityParams { get; set; }
    }
}
