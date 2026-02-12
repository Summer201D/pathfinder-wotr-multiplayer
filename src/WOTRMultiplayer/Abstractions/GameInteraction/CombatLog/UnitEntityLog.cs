namespace WOTRMultiplayer.Abstractions.GameInteraction.CombatLog
{
    public class UnitEntityLog
    {
        public string UnitId { get; set; }

        public UnitEntityLog(string unitId)
        {
            UnitId = unitId;
        }
    }
}
