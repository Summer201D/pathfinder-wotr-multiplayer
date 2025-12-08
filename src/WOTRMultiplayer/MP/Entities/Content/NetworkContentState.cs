using System.Collections.Generic;

namespace WOTRMultiplayer.MP.Entities.Content
{
    public class NetworkContentState
    {
        public List<NetworkDLC> DLCs { get; set; } = [];

        public List<NetworkMod> Mods { get; set; } = [];
    }
}
