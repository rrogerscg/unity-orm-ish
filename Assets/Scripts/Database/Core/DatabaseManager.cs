using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ORMish
{
    public class DatabaseManager
    {
        private static DatabaseManager _instance;
        private static readonly object _lock = new object();

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
        }

        public static DatabaseManager Initialize(string tablesPath)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DatabaseManager(tablesPath);
                        List<Type> recordTypes = Instance.GetAllRecordTypes();
                        if (recordTypes.Count > 0)
                        {
                            foreach (Type recordType in recordTypes)
                            {
                                Console.WriteLine($"Reflection found the following record types: {recordType.FullName}");
                            }
                            foreach (Type recordType in recordTypes)
                            {
                                MethodInfo m_recordConnectToTable = recordType.BaseType.GetMethod("ConnectToTable", BindingFlags.Static | BindingFlags.Public);
                                Console.WriteLine($"Table property: {m_recordConnectToTable}");
                                if (m_recordConnectToTable != null)
                                {
                                    m_recordConnectToTable?.Invoke(null, null);
                                    Console.WriteLine($"Instantiating table for record type {recordType.Name}");
                                }

                            }
                        }
                        else
                        {
                            Console.WriteLine("No models found in project");
                        }

                        TableRegistry.Instance.PrintRegistry();
                    }
                }
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

        public List<Type> GetAllRecordTypes()
        {
            List<Type> derivedTypes = ReflectionHelper.GetDerivedTypes(typeof(Record<>));
            return derivedTypes;
        }

        public List<string> GetAllTableNames()
        {
            return GetAllRecordTypes().Select(t => t.Name).ToList();
        }
    }
}