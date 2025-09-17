using System;
using System.Collections.Generic;

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
            Console.WriteLine("TableRegistry contains the following tables");
            foreach(KeyValuePair<string, ITable> kvp in _tablesByTableName)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }
    }
}

