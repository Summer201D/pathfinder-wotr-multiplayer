using System.Linq;
using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
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

        [ProtoMember(4)]
        public NetworkMultiplayerSettings Multiplayer { get; set; }

        public override string ToString()
        {
            var turnBased = DumpProperties(TurnBased);
            var main = DumpProperties(Main);
            var autoPause = DumpProperties(Autopause);
            var multiplayer = DumpProperties(Multiplayer);
            return $"TurnBased[{turnBased}], Main[{main}], Autopause[{autoPause}, Multiplayer[{multiplayer}]]";
        }

        private string DumpProperties(object obj)
        {
            return string.Join(";", obj.GetType().GetProperties().Select(x => string.Join("=", x.Name, x.GetValue(obj))));
        }
    }
}
