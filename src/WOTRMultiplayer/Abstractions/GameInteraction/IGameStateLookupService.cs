using Kingmaker.EntitySystem.Entities;

namespace WOTRMultiplayer.Abstractions.GameInteraction
{
    public interface IGameStateLookupService
    {
        UnitEntityData GetUnitEntity(string uniqueId);

        MapObjectEntityData GetMapObject(string uniqueId);
    }
}
