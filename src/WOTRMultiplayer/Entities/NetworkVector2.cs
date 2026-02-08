namespace WOTRMultiplayer.Entities
{
    public class NetworkVector2
    {
        public float X { get; set; }

        public float Y { get; set; }

        public NetworkVector2()
        {
        }

        public NetworkVector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"<{X},{Y}>";
        }
    }
}
