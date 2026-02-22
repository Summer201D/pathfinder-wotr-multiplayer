using System;
using System.Collections.Generic;
using System.Linq;

namespace WOTRMultiplayer.Extensions
{
    public static class EnumExtensions
    {
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes.FirstOrDefault();
        }

        public static List<T> GetAllFlags<T>(this T enumValue)
            where T : Enum
        {
            var flags = Enum.GetValues(typeof(T)).Cast<T>().Where(x => enumValue.HasFlag(x)).ToList();
            return flags;
        }
    }
}
