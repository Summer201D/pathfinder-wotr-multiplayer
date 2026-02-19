namespace WOTRMultiplayer.Entities.Combat
{
    public enum NetworkCombatTurnStage
    {
        Initialization,
        Starting,
        StartSynchronization,
        Playing,
        Ending,
        EndSynchronization,
        Ended
    }
}
