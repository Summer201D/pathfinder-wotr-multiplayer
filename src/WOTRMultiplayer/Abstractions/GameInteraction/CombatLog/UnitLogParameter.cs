namespace WOTRMultiplayer.Abstractions.GameInteraction.CombatLog
{
    public class UnitLogParameter
    {
        public string UnitId { get; set; }

        public UnitLogParameter(string unitId)
        {
            UnitId = unitId;
        }
    }
}
