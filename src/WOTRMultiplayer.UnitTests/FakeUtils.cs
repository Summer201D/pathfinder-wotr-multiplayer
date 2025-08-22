using System;
using System.Linq;
using FakeItEasy;
using WOTRMultiplayer.Networking.Abstractions;

namespace WOTRMultiplayer.UnitTests
{
    public static class FakeUtils
    {
        public static Action<T> GetRegisteredHandler<T>(INetworkClient networkClientFake)
        {
            var calls = Fake.GetCalls(networkClientFake).ToList();
            var setupHandlerCalls = calls.Where(x => x.Method.IsGenericMethod).ToList();
            var targetHandler = setupHandlerCalls.FirstOrDefault(x => x.Method.GetGenericArguments().Any(x => x == typeof(T)))?.Arguments.First() as Action<T>;
            return targetHandler;
        }

        public static Action<long, T> GetRegisteredHandler<T>(INetworkServer networkServerFake)
        {
            var calls = Fake.GetCalls(networkServerFake).ToList();
            var setupHandlerCalls = calls.Where(x => x.Method.IsGenericMethod).ToList();
            var targetHandler = setupHandlerCalls.FirstOrDefault(x => x.Method.GetGenericArguments().Any(x => x == typeof(T)))?.Arguments.First() as Action<long, T>;
            return targetHandler;
        }
    }
}
