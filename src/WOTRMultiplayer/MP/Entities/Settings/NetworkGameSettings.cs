using System.Linq;

namespace WOTRMultiplayer.MP.Entities.Settings
{
    public class NetworkGameSettings
    {
        public NetworkTurnBasedSettngs TurnBased { get; set; }

        public NetworkGameMainSettings Main { get; set; }

        public NetworkAutopauseSettings Autopause { get; set; }

        public override string ToString()
        {
            var turnBased = DumpProperties(TurnBased);
            var main = DumpProperties(Main);
            var autoPause = DumpProperties(Autopause);
            return $"TurnBased[{turnBased}], Main[{main}], Autopause[{autoPause}]";
        }

        private string DumpProperties(object obj)
        {
            return string.Join(";", obj.GetType().GetProperties().Select(x => string.Join("=", x.Name, x.GetValue(obj))));
        }
    }
}
