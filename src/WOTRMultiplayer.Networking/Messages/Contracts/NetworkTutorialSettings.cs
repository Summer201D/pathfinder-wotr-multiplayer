using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkTutorialSettings
    {
        [ProtoMember(1)]
        public bool ShowArmiesTutorial { get; set; }

        [ProtoMember(2)]
        public bool ShowBasicTutorial { get; set; }

        [ProtoMember(3)]
        public bool ShowContextTutorial { get; set; }

        [ProtoMember(4)]
        public bool ShowControlsAdvancedTutorial { get; set; }

        [ProtoMember(5)]
        public bool ShowControlsBasicTutorial { get; set; }

        [ProtoMember(6)]
        public bool ShowCrusadeTutorial { get; set; }

        [ProtoMember(7)]
        public bool ShowGameplayAdvancedTutorial { get; set; }

        [ProtoMember(8)]
        public bool ShowGameplayBasicTutorial { get; set; }

        [ProtoMember(9)]
        public bool ShowPathfinderRulesTutorial { get; set; }
    }
}
