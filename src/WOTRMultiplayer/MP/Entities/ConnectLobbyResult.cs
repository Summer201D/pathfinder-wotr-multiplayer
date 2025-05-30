namespace WOTRMultiplayer.MP.Entities
{
    public class ConnectLobbyResult
    {
        public bool IsOk { get; set; }

        public string Message { get; set; }

        public static ConnectLobbyResult Error(string message)
        {
            return new ConnectLobbyResult
            {
                Message = message,
                IsOk = false
            };
        }

        public static ConnectLobbyResult Ok()
        {
            return new ConnectLobbyResult
            {
                IsOk = true
            };
        }
    }
}
