using Kingmaker.UnitLogic.Parts;

namespace WOTRMultiplayer.Entities.Units.Parts
{
    public class NetworkUnitPartInPit
    {
        public float CurrentRoundSeconds { get; set; }

        public UnitInPitState State { get; set; }
    }
}
