namespace WOTRMultiplayer.Entities.Units
{
    public class NetworkUnitDescriptor
    {
        public int Damage { get; set; }

        public NetworkCharacterStats Stats { get; set; }

        public NetworkUnitState State { get; set; }
    }
}
