using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ORMish
{
    public class DatabaseManager
    {
        private static DatabaseManager _instance;
        private static string _tablesPath;
        public static Dictionary<string, ITable<IRecord>> _modelByModelName = new();

        public static DatabaseManager Instance
        {
            get
            {

                if (_instance == null)
                {
                    _instance = new DatabaseManager();
                    Console.WriteLine("DatabaseManager created.");
                }

                return _instance;
            }
        }


        public void Initialize(string tablesPath)
        {
            _tablesPath = tablesPath;
            if (!Directory.Exists(_tablesPath))
            {
                Directory.CreateDirectory(_tablesPath);
                Console.WriteLine("[DatabaseTests] Tables Directory created: " + _tablesPath);
            }
            UserCharacter.Table = new Table<UserCharacter>(_tablesPath);
            Score.Table = new Table<Score>(_tablesPath);

            TableRegistry.PrintRegistry();
        }

        public static void DeleteDatabase()
        {
            if(Directory.Exists(_tablesPath))
            {
                Directory.Delete(_tablesPath, true);
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