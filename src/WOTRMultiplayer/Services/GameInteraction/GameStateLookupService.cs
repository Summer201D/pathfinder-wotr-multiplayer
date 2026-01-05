using System;
using System.Linq;
using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using WOTRMultiplayer.Abstractions.GameInteraction;

namespace WOTRMultiplayer.Services.GameInteraction
{
    public class GameStateLookupService : IGameStateLookupService
    {
        public MapObjectEntityData GetMapObject(string uniqueId)
        {
            return Game.Instance.State.MapObjects.All.FirstOrDefault(o => string.Equals(o.UniqueId, uniqueId, StringComparison.OrdinalIgnoreCase));
        }

        public UnitEntityData GetUnitEntity(string uniqueId)
        {
            if (string.IsNullOrEmpty(uniqueId))
            {
                return null;
            }

            return Game.Instance.State.Units.All.FirstOrDefault(u => string.Equals(u.UniqueId, uniqueId, StringComparison.OrdinalIgnoreCase));
        }
    }
}
