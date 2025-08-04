using System.Collections.Generic;

namespace WOTRMultiplayer.MP.Entities.MapObjects
{
    public class NetworkOvertip
    {
        public string MapObjectId { get; set; }

        public List<string> Units { get; set; } = [];
    }
}
