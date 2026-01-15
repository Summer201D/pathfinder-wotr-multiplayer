using WOTRMultiplayer.Abstractions.GameInteraction;
using WOTRMultiplayer.Entities;
using WOTRMultiplayer.Entities.Ping;

namespace WOTRMultiplayer.Playground.Core.Dummies
{
    public class DummyPingInteractionService : IPingInteractionService
    {
        public void Create(NetworkPlayer player, NetworkPing ping)
        {
        }

        public NetworkPing Get()
        {
            return null;
        }
    }
}
