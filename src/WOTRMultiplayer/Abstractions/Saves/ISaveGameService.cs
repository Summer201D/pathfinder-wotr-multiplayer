using Kingmaker.EntitySystem.Persistence;

namespace WOTRMultiplayer.Abstractions.Saves
{
    public interface ISaveGameService
    {
        string GetSaveGamePath();

        SaveInfo LoadSave(string path);
    }
}
