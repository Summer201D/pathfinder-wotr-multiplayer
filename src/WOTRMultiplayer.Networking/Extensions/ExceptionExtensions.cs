using System;

namespace WOTRMultiplayer.Networking.Extensions
{
    public static class ExceptionExtensions
    {
        public static bool HasInner<T>(this Exception exception, out T inner)
            where T : Exception
        {
            while (exception != null)
            {
                if (exception is T innerEx)
                {
                    inner = innerEx;
                    return true;
                }

                exception = exception.InnerException;
            }

            inner = default;
            return false;
        }
    }
}
