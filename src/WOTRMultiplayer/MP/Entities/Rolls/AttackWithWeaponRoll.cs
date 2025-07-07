namespace WOTRMultiplayer.MP.Entities.Rolls
{
    public class AttackWithWeaponRoll : NetworkDiceRoll
    {
        public int AttackNumber { get; set; }

        public int CombatRound { get; set; }

        public override string GetIdString()
        {
            return base.GetIdString() + AttackNumber.ToString() + CombatRound.ToString();
        }
    }
}
