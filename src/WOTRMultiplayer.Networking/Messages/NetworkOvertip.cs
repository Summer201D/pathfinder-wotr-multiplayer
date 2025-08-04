using System.Collections.Generic;
using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages
{
    [ProtoContract]
    public class NetworkOvertip
    {
        [ProtoMember(1)]
        public string MapObjectId { get; set; }

        [ProtoMember(2)]
        public List<string> Units { get; set; } = [];
    }
}
