namespace WOTRMultiplayer.Entities.Items
{
    public class NetworkLootableEntity
    {
        public string Id { get; set; }

        public NetworkVector3 Position { get; set; }

        public NetworkLootableEntityType Type { get; set; }
    }
}
