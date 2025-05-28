namespace WOTRMultiplayer.UI
{
    /// <summary>
    /// should be replaced with localization later on
    /// </summary>
    public class StringConsts
    {
        public class MainMenu
        {
            public const string MultiplayerMenu = "Multiplayer";
        }

        public class MultiplayerWindow
        {
            public const string HostMenuLabel = "Host";
            public const string JoinMenuLabel = "Join";

            public class JoinMenu
            {
                public const string JoinButtonLabel = "Join";
                public const string ServerInputPlaceholder = "Enter Server IP address";
            }

            public class HostMenu
            {
                public const string HostButtonLabel = "Host";
                public const string HostButtonActiveLabel = "Select save";
                public const string ReadyButtonLabel = "Ready";
                public const string ReadyNotReadyButtonLabel = "Not Ready";
                public const string StartButtonLabel = "Start";
            }
        }

        public class LobbyInfoWindow
        {
            public const string ServerInfoSectionTitle = "Server";
            public const string PlayersSectionTitle = "Players";
            public const string CharactersSectionTitle = "Characters";
        }
    }
}
