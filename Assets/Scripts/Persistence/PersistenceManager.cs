using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace ORMish
{
    /*
     * Manages the in-game persistence state of the character, as well as the between sessions states
     * _playerDataPersistence refers to the saving and loading of the data to and from a file
     * _playerData refers to the in-memory data
     */
    public class PersistenceManager
    {
        private static PersistenceManager _instance;
        private DatabaseManager _databaseManager = DatabaseManager.Instance;
        private User _activeUser;
        public User ActiveUser => _activeUser;

        public static PersistenceManager Instance
        {
            get
            {
                _instance ??= new PersistenceManager();
                return _instance;
            }
        }

        public void Initialize()
        {
            _databaseManager.Initialize(Path.Combine(Application.persistentDataPath, "tables"));
            List<User> users = User.GetAll();
            foreach(User user in users)
            {
                Debug.Log("User found: " + user.Name);
                Debug.Log("User id: " + user.Id);
                Debug.Log("User IsActive: " + user.IsActive);
            }
            _activeUser = User.GetActiveUser();
            if (_activeUser == null)
            {
                Debug.Log("No active user found");
            }
            else
            {
                Debug.Log("Active user found: " + _activeUser.Name);
            }
        }

        public void SaveLevelData(Level level)
        {
            Debug.Log("Saving level data");
        }

        public void Save()
        {
            Debug.Log("Saving Database Tables");
            User.SaveTable();
            Score.SaveTable();
        }
    }
}
