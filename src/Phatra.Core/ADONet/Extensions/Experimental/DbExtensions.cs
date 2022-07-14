using System.Collections.Generic;

namespace Phatra.Core.AdoNet.Extensions.Experimental
{
    public static class DbExtensions
    {
        /// <summary>
        /// Please note: this is an experimental feature, API may change or be removed in future versions"
        /// </summary>
        public static void Insert<T>(this IDatabase db, IEnumerable<T> items)
        {
            var query = Query<T>.Create(((Database)db).MappingConvention).Insert;
            Do(db, items, query);
        }

        /// <summary>
        /// Please note: this is an experimental feature, API may change or be removed in future versions"
        /// </summary>
        public static void Update<T>(this IDatabase db, IEnumerable<T> items)
        {
            var query = Query<T>.Create(((Database)db).MappingConvention).Update;
            Do(db, items, query);
        }

        /// <summary>
        /// Please note: this is an experimental feature, API may change or be removed in future versions"
        /// </summary>
        public static void Delete<T>(this IDatabase db, IEnumerable<T> items)
        {
            var query = Query<T>.Create(((Database)db).MappingConvention).Delete;
            Do(db, items, query);
        }

        private static void Do<T>(IDatabase db, IEnumerable<T> items, string query)
        {
            var commandBuilder = db.Sql(query);
            foreach (var item in items)
            {
                commandBuilder.WithParameters(item).AsNonQuery();
            }
        }

    }
}
