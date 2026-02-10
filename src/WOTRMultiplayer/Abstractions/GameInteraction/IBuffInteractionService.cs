using Kingmaker.EntitySystem.Entities;
using WOTRMultiplayer.Entities.Units;

namespace WOTRMultiplayer.Abstractions.GameInteraction
{
    public interface IBuffInteractionService
    {
        NetworkUnitBuffCollection GetUnitBuffs(UnitEntityData combatUnit);

        void UpdateUnitBuffs(UnitEntityData unit, NetworkUnitBuffCollection unitBuffCollection);
    }
}
