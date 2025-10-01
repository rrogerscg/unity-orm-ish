using System.Collections.Generic;
using System.IO;
using ORMish;
using UnityEngine;


namespace Example
{
    /*
     * Manages the in-game persistence state of the character, as well as the between sessions states
     * _playerDataPersistence refers to the saving and loading of the data to and from a file
     * _playerData refers to the in-memory data
     */
    public class PersistenceManager
    {
        private static PersistenceManager _instance;
        private static readonly object _lock = new();

        public static PersistenceManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            Debug.Log($"Application persistence path is set to {Application.persistentDataPath}");
                            DatabaseManager.Initialize(Path.Combine(Application.persistentDataPath, "tables"));
                            _instance = new PersistenceManager();
                        }
                    }
                }
                return _instance;
            }
        }

        private List<UserCharacter> _userCharacters = new();
        public List<UserCharacter> UserCharacters => _userCharacters;
        private UserCharacter _activeUserCharacter;
        public UserCharacter ActiveUserCharacter => _activeUserCharacter;

        public bool UserCharactersExist => UserCharacters.Count > 0;

        public void SaveLevelData(Level level)
        {
            Debug.Log("Saving level data");
        }

        public void Save()
        {
            Debug.Log("Saving Database Tables");
            UserCharacter.SaveTable();
            Score.SaveTable();
        }

        public void LoadUserCharacters()
        {
            _userCharacters = UserCharacter.GetAll();
            Debug.Log($"{_userCharacters.Count} User Characters found");
            _activeUserCharacter = UserCharacter.GetActiveCharacter();
            if (_activeUserCharacter == null)
            {
                Debug.Log("No active user character found");
            }
            else
            {
                Debug.Log("Active user character found: " + _activeUserCharacter.Name);
            }
        }
    }
}
