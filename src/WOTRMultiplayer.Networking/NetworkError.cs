namespace WOTRMultiplayer.Networking
{
    public class NetworkError
    {
        public NetworkErrorType Type { get; private set; }

        public string Reason { get; set; }

        public NetworkError(NetworkErrorType type)
        {
            Type = type;
        }
    }
}
