namespace Phatra.Core.Infrastructure
{
    public class ApplicationContext : BaseContext
    {
        private ApplicationContext()
        {
        }

        private static ApplicationContext _Instance;

        public static ApplicationContext Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ApplicationContext();
                }
                return _Instance;
            }
        }

        protected override IEngine CreateDefaultEngine()
        {
            return new ApplicationEngine();
        }

    }
}
