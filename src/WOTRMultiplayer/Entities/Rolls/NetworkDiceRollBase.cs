using System.Collections.Generic;

namespace WOTRMultiplayer.Entities.Rolls
{
    public abstract class NetworkDiceRollBase
    {
        protected const string IdSeparator = ":";

        public string InitiatorId { get; private set; }

        public string RuleName { get; private set; }

        public int TotalModifiersBonus { get; private set; }

        public NetworkDiceRollType RollType { get; private set; }

        public NetworkDiceRollBase(string initiatorId, string ruleName, NetworkDiceRollType networkDiceRollType, int totalModifierBonus)
        {
            InitiatorId = initiatorId;
            RuleName = ruleName;
            RollType = networkDiceRollType;
            TotalModifiersBonus = totalModifierBonus;
        }

        public string GetIdString()
        {
            var baseId = string.Join(IdSeparator, GetBaseRollIdentifier());
            var id = string.Join(IdSeparator, GetRollIdentifier());
            var fullId = string.Join(IdSeparator, baseId, id);
            return fullId;
        }

        protected abstract IEnumerable<string> GetRollIdentifier();

        private IEnumerable<string> GetBaseRollIdentifier()
        {
            return [GetType().Name, RollType.ToString(), InitiatorId, RuleName, TotalModifiersBonus.ToString()];
        }
    }
}
