using System.Collections.Generic;

namespace WOTRMultiplayer.Entities.Rolls
{
    public class AttackRoll : NetworkDiceRollBase
    {
        public AttackWithWeaponRoll AttackWithWeapon { get; set; }

        public string AttackType { get; set; }

        public string TargetId { get; set; }

        public bool IsCriticalRoll { get; set; }

        public int FortificationChance { get; set; }

        public int MissChance { get; set; }

        public AttackRoll(string initiatorId, string ruleName, NetworkDiceRollType networkDiceRollType, int totalModifierBonus)
            : base(initiatorId, ruleName, networkDiceRollType, totalModifierBonus)
        {
        }

        public IEnumerable<string> GetIdentifier()
        {
            return GetRollIdentifier();
        }

        protected override IEnumerable<string> GetRollIdentifier()
        {
            var attackWithWeaponIdentifier = AttackWithWeapon == null ? null : string.Join(IdSeparator, AttackWithWeapon.GetIdentifier());

            return ["@", AttackType, TargetId, IsCriticalRoll.ToString(), FortificationChance.ToString(), MissChance.ToString(), "@", attackWithWeaponIdentifier];
        }
    }
}
