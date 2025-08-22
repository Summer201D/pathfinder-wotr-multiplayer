using FakeItEasy.Core;
using WOTRMultiplayer.Networking.Abstractions;

namespace WOTRMultiplayer.UnitTests.FakeRules
{
    public class NetworkServerFakeRule : IFakeObjectCallRule
    {
        public int? NumberOfTimesToCall => null;

        public void Apply(IInterceptedFakeObjectCall fakeObjectCall)
        {
            fakeObjectCall.SetReturnValue(fakeObjectCall.FakedObject);
        }

        public bool IsApplicableTo(IFakeObjectCall fakeObjectCall)
        {
            return fakeObjectCall.Method.DeclaringType == typeof(INetworkServer)
                    && fakeObjectCall.Method.Name == "On";
        }
    }
}
