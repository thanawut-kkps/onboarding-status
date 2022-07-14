using System.Collections.Generic;
using System.Linq;
using Phatra.Core.AdoNet;
using Phatra.Core.Logging;

namespace Phatra.Core.Managers
{
    public abstract class BaseManager
    {
        public ILogger Logger { get; set; }

        protected List<ColumnInfo> GetUserDefinedTableTypeColumnNameList(string databaseName, string userDefinedTableTypeName)
        {
            List<ColumnInfo> list;

            string cmdText = string.Format("select tt.name as table_name, c.name as column_name, c.column_id as column_id from {0}sys.table_types tt inner join {0}sys.columns c on c.object_id = tt.type_table_object_id where tt.name = @typeName order by c.column_id;", string.IsNullOrWhiteSpace(databaseName) ? "": string.Format("[{0}].", databaseName));

            using (var db = DatabaseFactory.CreateDefaultDatabase())
            {
                var query = db.Sql(cmdText)
                            .WithParameter("@typeName", userDefinedTableTypeName);

                list = query.AsEnumerable().Select(row => new ColumnInfo
                {
                    ColumnId =  row.column_id,
                    ColumnName = row.column_name
                }).ToList();
            }

            return list;
        }

    }

    public class ColumnInfo
    {
        public int ColumnId { get; set; }
        public string ColumnName { get; set; }
    }
}
