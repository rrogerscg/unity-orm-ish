using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ORMish
{
    public class DatabaseManager
    {
        private static DatabaseManager _instance;
        private readonly string _tablesPath;
        public string TablesPath => _tablesPath;
        public static Dictionary<string, ITable<IRecord>> _modelByModelName = new();

        private DatabaseManager(string tablesPath)
        {
            _tablesPath = tablesPath;
            if (!Directory.Exists(_tablesPath))
            {
                Directory.CreateDirectory(_tablesPath);
                Console.WriteLine("[DatabaseTests] Tables Directory created: " + _tablesPath);
            }

            TableRegistry.PrintRegistry();
        }

        public static DatabaseManager Initialize(string tablesPath)
        {
            if (_instance == null)
            {
                _instance = new DatabaseManager(tablesPath);
                Console.WriteLine("DatabaseManager created.");
            }

            return _instance;
        }

        public static DatabaseManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("DatabaseManager not initialized yet.");
                }
                return _instance;
            }
        }

        public static void DeleteDatabase()
        {
            if(Directory.Exists(Instance.TablesPath))
            {
                Directory.Delete(Instance.TablesPath, true);
            }
        }

        public List<string> GetAllTableNames()
        {
            List<Type> derivedTypes = ReflectionHelper.GetDerivedTypes(typeof(Record<>));
            return derivedTypes
                .Select(t => t.Name)
                .ToList();
        }
    }
}