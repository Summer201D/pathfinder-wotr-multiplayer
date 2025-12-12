using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkDialogPopup
    {
        [ProtoMember(1)]
        public string AreaName { get; set; }

        [ProtoMember(2)]
        public string DialogName { get; set; }

        [ProtoMember(3)]
        public string CueName { get; set; }
    }
}
