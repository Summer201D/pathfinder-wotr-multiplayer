using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace WOTRMultiplayer.Localization
{
    [Description(RootKey)]
    public static class WellKnownKeys
    {
        public const string KeyPathSeparator = ".";
        public const string RootKey = "wotrmultiplayer";

        public static void Initialize()
        {
            var typesToProcess = new Stack<(string, Type)>();
            var rootType = typeof(WellKnownKeys);
            var rootPath = rootType.GetCustomAttribute<DescriptionAttribute>().Description;
            typesToProcess.Push((rootPath, rootType));
            while (typesToProcess.Count > 0)
            {
                var (currentPath, currentType) = typesToProcess.Pop();
                var children = currentType.GetNestedTypes().Where(n => n.IsClass && n.GetCustomAttribute<DescriptionAttribute>() != null).ToList();
                if (children.Count > 0)
                {
                    foreach (var child in children)
                    {
                        var childPath = string.Join(KeyPathSeparator, currentPath, child.GetCustomAttribute<DescriptionAttribute>().Description);
                        typesToProcess.Push((childPath, child));
                    }

                    continue;
                }

                var keyProperty = currentType.GetProperty("Key")
                    ?? throw new InvalidOperationException($"A well-known key type has neither children nor Key property. Type={currentType}");

                keyProperty.SetValue(null, currentPath);
            }
        }

        [Description("multiplaterWindow")]
        public static class MultiplayerWindow
        {
            [Description("hostMenu")]
            public static class HostMenu
            {
                [Description("title")]
                public static class Title
                {
                    public static string Key { get; set; }
                }

                [Description("hostButton")]
                public static class HostButton
                {

                    [Description("hostText")]
                    public static class HostText
                    {
                        public static string Key { get; set; }
                    }

                    [Description("selectSaveText")]
                    public static class SelectSaveText
                    {
                        public static string Key { get; set; }
                    }
                }

                [Description("readyButton")]
                public static class ReadyButton
                {
                    [Description("readyText")]
                    public static class ReadyText
                    {
                        public static string Key { get; set; }
                    }

                    [Description("notReadyText")]
                    public static class NotReadyText
                    {
                        public static string Key { get; set; }
                    }
                }

                [Description("startButton")]
                public static class StartButton
                {
                    public static string Key { get; set; }
                }
            }

            [Description("joinMenu")]
            public static class JoinMenu
            {
                [Description("title")]
                public static class Title
                {
                    public static string Key { get; set; }
                }

                [Description("joinButton")]
                public static class JoinButton
                {
                    public static string Key { get; set; }
                }

                [Description("readyButton")]
                public static class ReadyButton
                {
                    [Description("readyText")]
                    public static class ReadyText
                    {
                        public static string Key { get; set; }
                    }

                    [Description("notReadyText")]
                    public static class NotReadyText
                    {
                        public static string Key { get; set; }
                    }
                }

                [Description("leaveButton")]
                public static class LeaveButton
                {
                    public static string Key { get; set; }
                }

                [Description("serverAddress")]
                public static class ServerAddress
                {
                    [Description("placeholder")]
                    public static class Placeholder
                    {
                        public static string Key { get; set; }
                    }
                }
            }
        }

        [Description("mainMenu")]
        public static class MainMenu
        {
            [Description("multiplayer")]
            public static class Multiplayer
            {
                [Description("title")]
                public static class Title
                {
                    public static string Key { get; set; }
                }
            }
        }

        [Description("escMenu")]
        public static class EscMenu
        {
            [Description("multiplayerLobby")]
            public static class MultiplayerLobby
            {
                [Description("title")]
                public static class Title
                {
                    public static string Key { get; set; }
                }
            }
        }

        [Description("settings")]
        public static class Settings
        {
            [Description("title")]
            public static class Title
            {
                public static string Key { get; set; }
            }

            [Description("general")]
            public static class General
            {
                [Description("title")]
                public static class Title
                {
                    public static string Key { get; set; }
                }

                [Description("playerName")]
                public static class PlayerName
                {
                    [Description("title")]
                    public static class Title
                    {
                        public static string Key { get; set; }
                    }

                    [Description("tooltip")]
                    public static class Tooltip
                    {
                        public static string Key { get; set; }
                    }
                }
            }

            [Description("combat")]
            public static class Combat
            {
                [Description("title")]
                public static class Title
                {
                    public static string Key { get; set; }
                }

                [Description("aiSync")]
                public static class AISync
                {
                    [Description("title")]
                    public static class Title
                    {
                        public static string Key { get; set; }
                    }

                    [Description("tooltip")]
                    public static class Tooltip
                    {
                        public static string Key { get; set; }
                    }
                }
            }

            [Description("networking")]
            public static class Networking
            {
                [Description("title")]
                public static class Title
                {
                    public static string Key { get; set; }
                }

                [Description("hostPortStart")]
                public static class HostPortRangeStart
                {
                    [Description("title")]
                    public static class Title
                    {
                        public static string Key { get; set; }
                    }

                    [Description("tooltip")]
                    public static class Tooltip
                    {
                        public static string Key { get; set; }
                    }
                }

                [Description("hostPortEnd")]
                public static class HostPortRangeEnd
                {
                    [Description("title")]
                    public static class Title
                    {
                        public static string Key { get; set; }
                    }

                    [Description("tooltip")]
                    public static class Tooltip
                    {
                        public static string Key { get; set; }
                    }
                }
            }

            [Description("dangerZone")]
            public static class DangerZone
            {
                [Description("title")]
                public static class Title
                {
                    public static string Key { get; set; }
                }

                [Description("defaultForcedPauseTimeout")]
                public static class DefaultForcedPauseTimeout
                {
                    [Description("title")]
                    public static class Title
                    {
                        public static string Key { get; set; }
                    }

                    [Description("tooltip")]
                    public static class Tooltip
                    {
                        public static string Key { get; set; }
                    }
                }

                [Description("restEncounterForcedPauseTimeout")]
                public static class RestEncounterForcedPauseTimeout
                {
                    [Description("title")]
                    public static class Title
                    {
                        public static string Key { get; set; }
                    }

                    [Description("tooltip")]
                    public static class Tooltip
                    {
                        public static string Key { get; set; }
                    }
                }
            }
        }
    }
}
