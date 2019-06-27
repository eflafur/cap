using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GruppoCap.Core.Data
{
    public static class SubCollectionExtensions
    {
        // TO DATA TABLE (USING FASTMEMBER)
        public static DataTable ToDataTable<T>(this IEnumerable<T> list) where T : class, IEntity
        {
            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(list))
            {
                table.Load(reader);
            }

            return table;
        }

        // TO DATA TABLE (USING FASTMEMBER)
        public static DataTable ToDataTable<T>(this IEnumerable<T> list, String[] members) where T : class, IEntity
        {
            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(list, members))
            {
                table.Load(reader);
            }

            return table;
        }

        // TO DATA TABLE (USING FASTMEMBER)
        public static DataTable ToDataTable<T>(this IEnumerable<T> list, Dictionary<String, String> memberDictionary) where T : class, IEntity
        {
            DataTable table = new DataTable();
            String[] _members = memberDictionary.Keys.ToArray();

            using (var reader = ObjectReader.Create(list, _members))
            {
                table.Load(reader);
            }

            String _replacedColName = String.Empty;
            foreach(DataColumn c in table.Columns)
            {
                if(memberDictionary.ContainsKey(c.ColumnName))
                {
                    c.ColumnName = memberDictionary[c.ColumnName];
                }
            }

            return table;
        }
    }
}