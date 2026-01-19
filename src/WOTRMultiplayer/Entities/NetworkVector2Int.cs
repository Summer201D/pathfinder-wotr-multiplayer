namespace WOTRMultiplayer.Entities
{
    public class NetworkVector2Int
    {
        public int X { get; set; }

        public int Y { get; set; }

        public NetworkVector2Int()
        {
        }

        public NetworkVector2Int(int x, int y)
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
