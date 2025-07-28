using System.Collections.Generic;

namespace WOTRMultiplayer.MP.Entities.Rolls
{

    public abstract class NetworkDiceRollBase
    {
        protected const string IdSeparator = "::";

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
            var baseId = string.Join(IdSeparator, GetBaseUniquinessIdentifiers());
            var id = string.Join(IdSeparator, GetUniquinessIdentifiers());
            var fullId = string.Join(IdSeparator, baseId, id);
            return fullId;
        }

        private IEnumerable<string> GetBaseUniquinessIdentifiers()
        {
            return [GetType().Name, InitiatorId, RuleName, TotalModifiersBonus.ToString()];
        }

        protected abstract IEnumerable<string> GetUniquinessIdentifiers();
    }
}
