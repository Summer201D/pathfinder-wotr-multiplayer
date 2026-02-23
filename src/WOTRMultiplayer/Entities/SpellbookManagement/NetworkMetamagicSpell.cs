using System.Collections.Generic;
using WOTRMultiplayer.Entities.Combat;

namespace WOTRMultiplayer.Entities.SpellbookManagement
{
    public class NetworkMetamagicSpell
    {
        public string UnitId { get; set; }

        public NetworkAbility Ability { get; set; }

        public int HeightenLevel { get; set; }

        public List<int> MetamagicFeatures { get; set; } = [];

        public int? BorderNumber { get; set; }

        public int? DecorationColorNumber { get; set; }
    }
}
