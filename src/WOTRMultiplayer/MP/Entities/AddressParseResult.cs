namespace WOTRMultiplayer.MP.Entities
{
    public class AddressParseResult
    {
        public bool IsOk { get; set; }

        public string Message { get; set; }

        public static AddressParseResult Error(string message)
        {
            return new AddressParseResult
            {
                Message = message,
                IsOk = false
            };
        }

        public static AddressParseResult Ok()
        {
            return new AddressParseResult
            {
                IsOk = true
            };
        }
    }
}
