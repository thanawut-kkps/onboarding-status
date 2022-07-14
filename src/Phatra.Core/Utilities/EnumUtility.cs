using System;

namespace Phatra.Core.Utilities
{
    public class EnumUtility
    {
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
