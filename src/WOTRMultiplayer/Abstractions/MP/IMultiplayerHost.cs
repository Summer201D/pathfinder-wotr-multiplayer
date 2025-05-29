using System.Collections.Generic;
using WOTRMultiplayer.MP;

namespace WOTRMultiplayer.Abstractions.MP
{
    public interface IMultiplayerHost
    {
        void Start(string gameName, List<string> portraits, MultiplayerSettings multiplayerSettings);

        void Stop();

        bool ReadyChanged();

        void NotifyGameCharactersChanged(string saveGameName, List<string> portraits);

        bool IsActive { get; }
    }
}
