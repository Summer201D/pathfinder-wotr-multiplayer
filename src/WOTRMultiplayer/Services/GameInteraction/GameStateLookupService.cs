using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Globalmap.State;
using Kingmaker.Globalmap.View;
using Kingmaker.View.MapObjects;
using UnityEngine;
using WOTRMultiplayer.Abstractions.GameInteraction;
using WOTRMultiplayer.Entities;
using WOTRMultiplayer.Entities.GlobalMap;

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

        public List<MapObjectEntityData> GetNeareastLootableMapObjects(NetworkVector3 position)
        {
            var targetPoint = new Vector3(position.X, position.Y, position.Z);
            var orderedContainers = Game.Instance.State.MapObjects.All
                .Where(o => o.Interactions.Any(i => i is InteractionLootPart))
                .OrderBy(o => (o.Position - targetPoint).magnitude)
                .ToList();

            return orderedContainers;
        }

        public MapObjectEntityData GetNeareastLootBagMapObject(NetworkVector3 position)
        {
            var allNearest = GetNeareastLootableMapObjects(position);
            var lootbag = allNearest.FirstOrDefault(o => o is DroppedLoot.EntityData);
            return lootbag;
        }

        public GlobalMapPointView GetGlobalMapPoint(NetworkGlobalMapLocation globalMapLocation)
        {
            var point = GlobalMapView.Instance?
                .Points
                .FirstOrDefault(p => string.Equals(p.Blueprint.AssetGuid.ToString(), globalMapLocation.Id, StringComparison.OrdinalIgnoreCase));

            return point;
        }

        public GlobalMapArmyPawn GetGlobalMapArmyPawn(NetworkGlobalMapArmyPawn globalMapArmyPawn)
        {
            var armyPawn = GlobalMapView.Instance?.GetArmyView(globalMapArmyPawn.Id);
            return armyPawn;
        }

        public GlobalMapArmyState GetGlobalMapArmy(string id)
        {
            var map = Game.Instance.GlobalMapController.SelectedTraveler?.Location.GlobalMap;
            var state = Game.Instance.Player.GetGlobalMap(map);
            var army = state?.Armies.FirstOrDefault(a => string.Equals(a.Id, id, StringComparison.OrdinalIgnoreCase));
            return army;
        }
    }
}
