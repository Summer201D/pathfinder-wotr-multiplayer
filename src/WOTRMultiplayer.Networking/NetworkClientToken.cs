using BeetleX;

namespace WOTRMultiplayer.Networking
{
    public class NetworkClientToken : ISessionToken
    {
        public long Id { get; private set; }

        public void Dispose()
        {
        }

        public void Init(IServer server, ISession session)
        {
            Id = session.ID;
        }
    }
}
