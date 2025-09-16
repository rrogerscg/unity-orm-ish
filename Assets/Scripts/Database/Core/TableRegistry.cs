using System.Collections.Generic;
using UnityEngine;

namespace ORMish
{
    public static class TableRegistry
    {
        private static Dictionary<string, ITable> _tablesByTableName = new();
        public static IReadOnlyDictionary<string, ITable> TablesByTableName => _tablesByTableName;

        public static void Register(ITable table)
        {
            _tablesByTableName[table.Name] = table;
        }

        public static void PrintRegistry()
        {
            Debug.Log("TableRegistry contains the following tables");
            foreach(KeyValuePair<string, ITable> kvp in _tablesByTableName)
            {
                Debug.Log($"{kvp.Key}: {kvp.Value}");
            }
        }
    }
}

