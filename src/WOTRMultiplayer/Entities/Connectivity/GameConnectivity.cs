using System.Net;

namespace WOTRMultiplayer.Entities.Connectivity
{
    public class GameConnectivity
    {
        public EndPoint Endpoint { get; set; }

        public ExternalConnectivity External { get; set; }
    }
}
