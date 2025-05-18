using System;
using System.IO;
using System.Runtime.CompilerServices;
using WOTRMultiplayer.Config.UnityMod;
using WOTRMultiplayer.Natives;

namespace WOTRMultiplayer.Logging
{
    public static class Logger
    {
        private static TextWriter _output;

        public static void Initialize(UnityModManagerSettings settings)
        {
            _output = settings.UseDebugConsole ? SpawnConsole() : Console.Out;
        }

        private static TextWriter SpawnConsole()
        {
            var writer = WinApi.SpawnConsole();
            return writer;
        }

        public static void Info(string message, [CallerMemberName] string name = "")
        {
            Out(message, LogSeverity.Info, name);
        }

        private static void Out(string message, LogSeverity severity, string name)
        {
            _output.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] [{name}] [{severity}] - {message}");
        }

        private enum LogSeverity
        {
            Debug,
            Info,
            Error
        }
    }
}
