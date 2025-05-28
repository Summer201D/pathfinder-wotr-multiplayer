using System.Collections.Generic;
using WOTRMultiplayer.MP;

namespace WOTRMultiplayer.Abstractions.MP
{
    public interface IMultiplayerHost
    {
        void Start(MultiplayerSettings multiplayerSettings);

        void Stop();

        bool ReadyChanged();

        void NotifySaveChanged(string saveGameName, List<string> portraits);

        bool IsActive { get; }
    }
}
