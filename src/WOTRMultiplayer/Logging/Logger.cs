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

        public static void Info(string message, [CallerMemberName] string name = "")
        {
            Out(message, LogSeverity.Info, name);
        }

        public static void Error(Exception exception, [CallerMemberName] string name = "")
        {
            Out(exception.ToString(), LogSeverity.Error, name);
        }

        public static void Error(string message, [CallerMemberName] string name = "")
        {
            Out(message, LogSeverity.Error, name);
        }

        private static void Out(string message, LogSeverity severity, string name)
        {
            _output.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] [{name}] [{severity}] - {message}");
        }

        private static TextWriter SpawnConsole()
        {
            var writer = WinApi.SpawnConsole();
            return writer;
        }

        private enum LogSeverity
        {
            Debug,
            Info,
            Error
        }
    }
}
