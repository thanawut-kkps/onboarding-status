namespace Phatra.Core.AdoNet
{
    public class DatabaseFactory
    {
        public static Database CreateDefaultDatabase()
        {
            return Database.FromConfig("Database");
        }

        public static Database CreateWebCtrlDatabase()
                  => Database.FromConfig("WebCtrlDB");

    }
}
