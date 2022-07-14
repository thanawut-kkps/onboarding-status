using Phatra.Core.Infrastructure;

namespace Phatra.Core.Web.Infrastructure
{
    /// <summary>
    /// Provides access to the singleton instance of the web engine.
    /// </summary>
    public class WebContext : BaseContext
    {
        private WebContext()
        {
        }

        private static WebContext _Instance;

        public static WebContext Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new WebContext();
                }
                return _Instance;
            }
        }


        protected override IEngine CreateDefaultEngine()
        {
            return new WebEngine();
        }

    }
}
