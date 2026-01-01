using WOTRMultiplayer.Entities.Equipment;

namespace WOTRMultiplayer.Entities.Items
{
    public class NetworkPolymorphicItem
    {
        public string UnitId { get; set; }

        public NetworkEquipmentSlotPosition Position { get; set; }

        public NetworkItem Item { get; set; }
    }
}
