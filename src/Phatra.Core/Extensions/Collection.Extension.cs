using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace Phatra.Core.Extensions
{
    public static class CollectionExtension
    {
        //public static DataTable ToDataTable<TSource>(this IList<TSource> data)
        //{
        //    DataTable dataTable = new DataTable(typeof(TSource).Name);
        //    PropertyInfo[] props = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        //    List<string> columnNames = new List<string>();

        //    foreach (PropertyInfo prop in props)
        //    {
        //        if (prop.PropertyType.IsEnum)
        //        {
        //            columnNames.Add(prop.Name);
        //            dataTable.Columns.Add(prop.Name, typeof(Int16));
        //        }
        //        else if (!prop.PropertyType.IsClass)
        //        {
        //            columnNames.Add(prop.Name);
        //            dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ??
        //                            prop.PropertyType);
        //        }
        //    }

            

        //    foreach (TSource item in data)
        //    {
        //        DataRow dr = dataTable.NewRow();
        //        for (int i = 0; i < props.Length; i++)
        //        {       
        //            if (columnNames.Contains(props[i].Name))
        //            {                        
        //                dr[props[i].Name] = props[i].GetValue(item, null) ?? DBNull.Value;                        
        //            }                    
        //        }
        //        dataTable.Rows.Add(dr);
        //    }
        //    return dataTable;
        //}  

        /// <summary>
        /// Basic data types 
        /// </summary>
        private static Type[] dataTypes = {
            typeof(byte)
            ,typeof(sbyte)
            ,typeof(short)
            ,typeof(ushort)
            ,typeof(int)
            ,typeof(uint)
            ,typeof(long)
            ,typeof(ulong)
            ,typeof(float)
            ,typeof(double)
            ,typeof(decimal)
            ,typeof(bool)
            ,typeof(char)
            ,typeof(Guid)
            ,typeof(DateTime)
            ,typeof(DateTimeOffset)
            ,typeof(byte[])
            ,typeof(string)
        };

        /// <summary>
        /// Converts a generic List to a DataTable
        /// <see cref="http://stackoverflow.com/a/5805044"/>
        /// </summary>
        /// <typeparam name="T">Type of the object to convert to DataTable</typeparam>
        /// <param name="data">Data to be converted</param>
        /// <returns>The converted DataTable</returns>
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            IEnumerable<PropertyDescriptor> properties = 
                from x in TypeDescriptor.GetProperties(typeof(T))
                          .Cast<PropertyDescriptor>()
                          where IsBasicType(x.PropertyType)
                          select x;

            DataTable table = GetDataTable(data, properties);
            return table;
        }

        /// <summary>
        /// Converts a generic List to a DataTable
        /// <see cref="http://stackoverflow.com/a/5805044"/>
        /// </summary>
        /// <typeparam name="T">Type of the object to convert to DataTable</typeparam>
        /// <param name="data">Data to be converted</param>
        /// <returns>The converted DataTable</returns>
        public static DataTable ToDataTable<T>(this IList<T> data, 
            Func<PropertyDescriptor, bool> expression)
        {
            var properties = TypeDescriptor.GetProperties(typeof(T))
                .Cast<PropertyDescriptor>()
                .Where(expression);

            DataTable table = GetDataTable(data, properties);
            return table;
        }

        /// <summary>
        /// Converts an IEnumerable to a DataTable
        /// <see cref="http://stackoverflow.com/a/5805044"/>
        /// </summary>
        /// <typeparam name="T">Type of the object to convert to DataTable</typeparam>
        /// <param name="data">Data to be converted</param>
        /// <returns>The DataTable</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
        {
            return data.ToList().ToDataTable();
        }

        /// <summary>
        /// Converts an IEnumerable to a DataTable
        /// <see cref="http://stackoverflow.com/a/5805044"/>
        /// </summary>
        /// <typeparam name="T">Type of the object to convert to DataTable</typeparam>
        /// <param name="data">Data to be converted</param>
        /// <param name="expression">Predicate to filter the properties of <typeparamref name="T"/> to be included to the DataTable</param>
        /// <returns>The DataTable</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> data, 
            Func<PropertyDescriptor, bool> expression)
        {
            return data.ToList().ToDataTable(expression);
        }

        #region Private methods

        private static bool IsBasicType(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;

            return type.IsEnum || dataTypes.Contains(type);
        }

        private static DataTable GetDataTable<T>(this IList<T> data, 
            IEnumerable<PropertyDescriptor> mappedProperties)
        {
            DataTable table = new DataTable();

            // columns
            foreach(PropertyDescriptor prop in mappedProperties)
            {
                table.Columns.Add(prop.Name, 
                                 Nullable.GetUnderlyingType(prop.PropertyType) ?? 
                                 prop.PropertyType);
            }

            // row values
            foreach(T item in data)
            {
                DataRow row = table.NewRow();

                foreach(PropertyDescriptor prop in mappedProperties)
                {
                    object value = prop.GetValue(item) ?? DBNull.Value;
                    row[prop.Name] = value;
                }

                table.Rows.Add(row);
            }

            return table;
        }

        #endregion
    }
}
