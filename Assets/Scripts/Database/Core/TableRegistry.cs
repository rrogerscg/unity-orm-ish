using System;
using System.Collections.Generic;

namespace ORMish
{
    public class TableRegistry
    {
        private static TableRegistry _instance;
        private static readonly object _lock = new object();

        public static TableRegistry Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new TableRegistry();
                        }
                    }
                }
                return _instance;
            }
        }

        private Dictionary<string, ITable> _tablesByTableName = new();
        public IReadOnlyDictionary<string, ITable> TablesByTableName => _tablesByTableName;

        public void Register(ITable table)
        {
            Console.WriteLine($"Registering table {table.Name} to the table registry");
            _tablesByTableName[table.Name] = table;
        }

        public void PrintRegistry()
        {
            Console.WriteLine("TableRegistry contains the following tables");
            foreach(KeyValuePair<string, ITable> kvp in _tablesByTableName)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }
    }
}

