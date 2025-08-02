namespace WOTRMultiplayer.MP.Entities.Loot
{
    public class NetworkDropItem
    {
        public string OwnerEntityId { get; set; }

        public NetworkItem Item { get; set; }
    }
}
