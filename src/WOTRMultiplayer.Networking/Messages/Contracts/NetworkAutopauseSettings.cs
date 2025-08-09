using ProtoBuf;

namespace WOTRMultiplayer.Networking.Messages.Contracts
{
    [ProtoContract]
    public class NetworkAutopauseSettings
    {
        [ProtoMember(1)]
        public bool ContinueMovementOnEngagement { get; set; }

        [ProtoMember(2)]
        public bool PauseOnAllyDown { get; set; }

        [ProtoMember(3)]
        public bool PauseOnAreaLoaded { get; set; }

        [ProtoMember(4)]
        public bool PauseOnAttackOfOpportunity { get; set; }

        [ProtoMember(5)]
        public bool PauseOnEndedBuffSummon { get; set; }

        [ProtoMember(6)]
        public bool PauseOnEndOfPartyMembersRound { get; set; }

        [ProtoMember(7)]
        public bool PauseOnEndOfRound { get; set; }

        [ProtoMember(8)]
        public bool PauseOnEnemyDown { get; set; }

        [ProtoMember(9)]
        public bool PauseOnEnemySpotted { get; set; }

        [ProtoMember(10)]
        public bool PauseOnEngagement { get; set; }

        [ProtoMember(11)]
        public bool PauseOnHiddenObjectDetected { get; set; }

        [ProtoMember(12)]
        public bool PauseOnLostFocus { get; set; }

        [ProtoMember(13)]
        public bool PauseOnLowHealth { get; set; }

        [ProtoMember(14)]
        public bool PauseOnMeleeEngagement { get; set; }

        [ProtoMember(15)]
        public bool PauseOnNewEnemyAppeared { get; set; }

        [ProtoMember(16)]
        public bool PauseOnPartyIsAttacked { get; set; }

        [ProtoMember(17)]
        public bool PauseOnPartyMemberFinishedAbility { get; set; }

        [ProtoMember(18)]
        public bool PauseOnPartyMemberRanOutOfConsumable { get; set; }

        [ProtoMember(19)]
        public bool PauseOnSpellcastFinished { get; set; }

        [ProtoMember(20)]
        public string PauseOnSpellcastInterrupted { get; set; }

        [ProtoMember(21)]
        public string PauseOnSpellcastStarted { get; set; }

        [ProtoMember(22)]
        public bool PauseOnTrapDetected { get; set; }

        [ProtoMember(23)]
        public bool PauseOnWeaponIsIneffective { get; set; }

        [ProtoMember(24)]
        public bool PauseWhenAllyUnconscious { get; set; }

        [ProtoMember(25)]
        public bool PauseWhenEnemyUnconscious { get; set; }

        [ProtoMember(26)]
        public bool PauseWhenLastSleepingEnemyStays { get; set; }
    }
}
