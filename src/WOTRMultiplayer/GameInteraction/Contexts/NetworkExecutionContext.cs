using System;
using System.Collections.Generic;
using Kingmaker.EntitySystem.Entities;

namespace WOTRMultiplayer.GameInteraction.Contexts
{
    public class NetworkExecutionContext : IDisposable
    {
        public List<UnitEntityData> SelectedUnits { get; set; }

        public PerceptionCheckContext PerceptionCheck { get; set; }

        public DropItemContext DropItem { get; set; }

        public EquipmentContext Equipment { get; set; }

        public HandEquipmentContext HandEquipment { get; set; }

        public void Dispose()
        {
            SelectedUnits = null;
            PerceptionCheck = null;
            DropItem = null;
            Equipment = null;
            HandEquipment = null;
        }
    }
}
