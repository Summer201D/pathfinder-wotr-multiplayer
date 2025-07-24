using CommandLine;

namespace WOTRMultiplayer.Playground.Client
{
    /// <summary>
    /// verbs are listed in the same order
    /// </summary>
    public class CommandVerbs
    {
        [Verb("connect", HelpText = "connect to specified host, default is 127.0.0.1:1024")]
        public class ConnectCommandVerb
        {
            [Option('s', "server", Required = false, Default = "127.0.0.1:1024")]
            public string ServerAddress { get; set; }
        }

        [Verb("ready", HelpText = "triggers ready status change")]
        public class ReadyCommandVerb
        {
        }

        [Verb("loaded", HelpText = "send gameloaded to host")]
        public class ClientLoadedCommandVerb
        {
        }
    }
}
