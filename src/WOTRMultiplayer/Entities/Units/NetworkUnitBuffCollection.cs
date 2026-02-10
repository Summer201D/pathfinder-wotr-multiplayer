using System.Collections.Generic;

namespace WOTRMultiplayer.Entities.Units
{
    public class NetworkUnitBuffCollection
    {
        public List<NetworkBuff> Buffs { get; set; } = [];

        public List<NetworkUnitNegativeLevelsData> NegativeLevels { get; set; } = [];
    }
}
