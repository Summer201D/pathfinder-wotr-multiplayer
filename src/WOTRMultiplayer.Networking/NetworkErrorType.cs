namespace WOTRMultiplayer.Networking
{
    public enum NetworkErrorType
    {
        Generic,
        Disconnected,
        SocketError,

        ModConflict,
        UnreachableSignalingServer,
        GameHostUnavailable,
        GameNotFound,

        P2PTimeout
    }
}
