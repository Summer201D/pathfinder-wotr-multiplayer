using System;
using System.Linq;
using System.Reflection;
using CommandLine;

namespace WOTRMultiplayer.Playground.Core
{
    public static class CommandLineHelper
    {
        public static Type[] LoadVerbs()
        {
            return [.. Assembly.GetEntryAssembly().GetTypes().Where(t => t.GetCustomAttribute<VerbAttribute>() != null)];
        }
    }
}
