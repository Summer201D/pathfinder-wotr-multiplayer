namespace WOTRMultiplayer.Entities
{
    public class NetworkPlayer
    {
        public string Name { get; private set; }

        public NetworkPlayerReadinessStatus Status { get; private set; }

        public NetworkPlayer()
        {
        }

        public NetworkPlayer(string name, NetworkPlayerReadinessStatus networkPlayerReadinessStatus)
        {
            Name = name;
            Status = networkPlayerReadinessStatus;
        }
    }
}
