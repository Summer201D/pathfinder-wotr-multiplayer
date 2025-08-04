using System;
using System.Collections.Generic;
using Kingmaker.EntitySystem.Entities;

namespace WOTRMultiplayer.GameInteraction
{
    public class NetworkExecutionContext : IDisposable
    {
        public List<UnitEntityData> SelectedUnits { get; set; }

        public void Dispose()
        {
            SelectedUnits = null;
        }
    }
}
