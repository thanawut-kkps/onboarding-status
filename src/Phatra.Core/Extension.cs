namespace Phatra.Core
{
    public static class Extension
    {
        public static bool IsNullOrDefault<T>(this T? value) where T : struct
        {
            return default(T).Equals(value.GetValueOrDefault());
        }
    }
}
