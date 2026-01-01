namespace WOTRMultiplayer.Services.GameInteraction.Contexts
{
    public class PolymorphicItemContext
    {
        public string UnitId { get; set; }

        public string ItemName { get; set; }

        public PolymorphicItemContext(string unitId, string itemName)
        {
            UnitId = unitId;
            ItemName = itemName;
        }
    }
}
