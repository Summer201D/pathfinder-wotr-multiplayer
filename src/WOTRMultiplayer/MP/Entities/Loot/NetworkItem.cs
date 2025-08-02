namespace WOTRMultiplayer.MP.Entities.Loot
{
    public class NetworkItem
    {
        public string UniqueId { get; set; }

        public string BlueprintId { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }

        public int Cost { get; set; }

        public int EnchantmentValue { get; set; }

        public string FirstEnchantmentName { get; set; }

        public int EnchantmentsCount { get; set; }
    }
}
