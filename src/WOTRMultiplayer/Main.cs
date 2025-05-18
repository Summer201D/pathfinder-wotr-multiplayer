using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;
using WOTRMultiplayer.Config.UnityMod;
using static UnityModManagerNet.UnityModManager;

namespace WOTRMultiplayer
{
    public class Main
    {
        private static UnityModManagerSettings _settings;

        public static bool Load(UnityModManager.ModEntry entry)
        {
            _settings = UnityModManager.ModSettings.Load<UnityModManagerSettings>(entry);

            Logging.Logger.Initialize(_settings);

            entry.OnGUI += OnGui;
            entry.OnSaveGUI += OnSaveGui;
            entry.OnToggle += OnToggle;

            try
            {
                var harmony = new Harmony(entry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (System.Exception ex)
            {
                Logging.Logger.Error(ex);
                throw;
            }

            return true;
        }

        private static bool OnToggle(ModEntry entry, bool arg2)
        {
            return true;
        }

        private static void OnSaveGui(UnityModManager.ModEntry entry)
        {
            _settings.Save(entry);
        }

        private static void OnGui(UnityModManager.ModEntry entry)
        {
            UnityEngine.GUILayout.BeginHorizontal();
            _settings.UseDebugConsole = UnityEngine.GUILayout.Toggle(_settings.UseDebugConsole, $"Use Debug Console (requires restart)", UnityEngine.GUILayout.ExpandWidth(false));
            UnityEngine.GUILayout.EndHorizontal();
        }
    }
}
