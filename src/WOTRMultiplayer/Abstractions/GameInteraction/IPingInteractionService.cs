using WOTRMultiplayer.Entities;
using WOTRMultiplayer.Entities.Ping;

namespace WOTRMultiplayer.Abstractions.GameInteraction
{
    public interface IPingInteractionService
    {
        NetworkPing Get();

        void Create(NetworkPlayer player, NetworkPing ping);
    }
}
