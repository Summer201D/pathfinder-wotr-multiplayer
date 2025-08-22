using System.Net;

namespace WOTRMultiplayer.Networking.Abstractions
{
    public interface IIPEndPointParser
    {
        IPEndPoint Parse(string value);
    }
}
