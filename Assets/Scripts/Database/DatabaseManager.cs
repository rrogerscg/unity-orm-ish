using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace ORMish
{
    /// <summary>
    /// Manages all database tables.
    /// </summary>
    public class DatabaseManager
    {
        private static DatabaseManager _instance;
        private static string _tablesPath;

        public static DatabaseManager Instance
        {
            get
            {

                if (_instance == null)
                {
                    _instance = new DatabaseManager();
                    Debug.Log("DatabaseManager created.");
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
                Debug.Log("[DatabaseTests] Tables Directory created: " + _tablesPath);
            }
            User.Table = new Table<User>(_tablesPath);
            Score.Table = new Table<Score>(_tablesPath);
        }

        public static void DeleteDatabase()
        {
            if(Directory.Exists(_tablesPath))
            {
                Directory.Delete(_tablesPath, true);
            }
        }

        public void SaveAllRecords()
        {
            
        }
    }
}