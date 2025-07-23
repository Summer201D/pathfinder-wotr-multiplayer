using System.Collections.Generic;

namespace WOTRMultiplayer.MP.Entities.Rolls
{
    public class NetworkDiceRoll
    {
        protected const string IdSeparator = "::";

        public int Result { get; set; }

        public List<int> RollHistory { get; set; } = [];

        public string InitiatorId { get; set; }

        public string RuleRollType { get; set; }

        public string RuleRollName { get; set; }

        public int TotalModifiersBonus { get; set; }

        public List<NetworkDamageValueRoll> DamageValues { get; set; } = [];

        public virtual string GetIdString()
        {
            return string.Join(IdSeparator, GetType().Name, InitiatorId, RuleRollType, RuleRollName, TotalModifiersBonus);
        }

        public virtual bool IsCompleted()
        {
            return true;
        }
    }
}
