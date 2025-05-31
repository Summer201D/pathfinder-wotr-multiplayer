using Kingmaker;
using Kingmaker.EntitySystem.Persistence;
using Microsoft.Extensions.Logging;
using WOTRMultiplayer.Abstractions.Saves;

namespace WOTRMultiplayer.Saves
{
    public class SaveGameService : ISaveGameService
    {
        private readonly ILogger<SaveGameService> _logger;

        public SaveGameService(ILogger<SaveGameService> logger)
        {
            _logger = logger;
        }

        public string GetSaveGamePath()
        {
            var path = Game.Instance.SaveManager.SavePath;
            return path;
        }

        public SaveInfo LoadSave(string path)
        {
            var save = Game.Instance.SaveManager.LoadZipSave(path);
            return save;
        }
    }
}
