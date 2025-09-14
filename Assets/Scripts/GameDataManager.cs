using System;
using System.Collections.Generic;
using UnityEngine;
using static ORMish.SceneState;
using static ORMish.UnlockableScriptableObject;

namespace ORMish
{
    // GameDataManager is an objectPooler...
    public class GameDataManager : MonoBehaviour
    {
        private static GameDataManager _instance;

        public static GameDataManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<GameDataManager>();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            LoadGameData();
            DontDestroyOnLoad(gameObject);
        }

        private Dictionary<EGameNames, List<GameDataScriptableObject>> _gameDataByGameName;
        public Dictionary<EGameNames, List<GameDataScriptableObject>> GameDataByGameName => _gameDataByGameName;

        public void LoadGameData()
        {
            Debug.Log("Loading GameDataScriptableObjects");
            _gameDataByGameName = new();
            GameDataScriptableObject[] gameDataScriptableObjects = Resources.LoadAll<GameDataScriptableObject>("");
            foreach (EGameNames gameName in Enum.GetValues(typeof(EGameNames)))
            {
                _gameDataByGameName[gameName] = new List<GameDataScriptableObject>();
                foreach (GameDataScriptableObject gameData in gameDataScriptableObjects)
                {
                    if (gameData.GameName == gameName)
                    {
                        _gameDataByGameName[gameName].Add(gameData);
                    }
                }
            }
            Debug.Log("GameDataScriptableObjects Loaded");
        }

        // SceneState will pass in GameName to filter the unlockables by game
        public void GetGameDataScriptableObjectsByGameNameAndTheme(EGameNames gameName, ETheme theme, List<GameDataScriptableObject> gameDataScriptableObjects)
        {
            gameDataScriptableObjects.Clear();
            foreach (GameDataScriptableObject gameData in _gameDataByGameName[gameName])
            {
                Debug.Log($"{gameData.Theme} - {theme}");
                if (gameData.Theme == theme)
                {
                    gameDataScriptableObjects.Add(gameData);
                    Debug.Log($"Added to list of gameDataScriptableObjects: {gameData.GameName}");
                }
            }
        }
    }
}
