using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages
{
    [ProtoContract]
    public class NetworkGameSettings
    {
        [ProtoMember(1)]
        public NetworkTurnBasedSettngs TurnBased { get; set; }

        [ProtoMember(2)]
        public NetworkGameMainSettings Main { get; set; }

        [ProtoMember(3)]
        public NetworkAutopauseSettings Autopause { get; set; }
    }
}
