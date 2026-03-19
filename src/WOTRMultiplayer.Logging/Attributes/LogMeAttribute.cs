using System;

namespace WOTRMultiplayer.Logging.Attributes
{
    /// <summary>
    /// Marked property will be logged by a logger
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class LogMeAttribute : Attribute
    {
    }
}
