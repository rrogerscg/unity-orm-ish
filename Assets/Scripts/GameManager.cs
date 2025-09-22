using System.Collections.Generic;
using ORMish;
using UnityEngine;
using static Example.SceneState;

namespace Example
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        private static readonly object _lock = new object();
        private static PersistenceManager _persistenceManager;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = FindFirstObjectByType<GameManager>();
                        }
                        if (_instance == null)
                        {
                            Debug.LogError("No GameManager found in scene!");
                        }
                    }
                }
                return _instance;
            }
        }

        public Dictionary<bool, List<GameObject>> ThemeUnlockablesMap;
        public EGameNames ActiveGameName;

        private bool _userIsSubscribed;
        public bool UserIsSubscribed => _userIsSubscribed;

        [SerializeField]
        private SceneState _startingScene;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            _userIsSubscribed = false;
            DontDestroyOnLoad(gameObject);
            InputActionsMap.Initialize();
            _persistenceManager = PersistenceManager.Instance;
        }

        private void Start()
        {
            _persistenceManager.LoadUserCharacters();
            if (_persistenceManager.UserCharactersExist == false)
            {
                PersistenceEvents.UserCharactersExist?.Invoke(false);
            }
            TableRegistry.Instance.PrintRegistry();
        }

        public bool CharacterDataIsLoaded()
        {
            return PersistenceManager.Instance.ActiveUserCharacter != null;
        }

        public void SaveLevelData(Level level)
        {

            PersistenceManager.Instance.SaveLevelData(level);
        }

        // TODO: create loading screen
        public void ShowLoadingScreen()
        {
            //LoadingScreen.Instance.Show();
            Debug.Log("Some loading screen content...");
        }

        // TODO: add this to LoadingScreen.cs
        //public IEnumerator Loading()
        //{
        //    while (!_isFullyInitialized)
        //    {
        //        yield return null;
        //    }
        //    Debug.Log("All scenes are fully initialized.");
        //}

        public void ExitGame()
        {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitGame();
            }
        }
    }
}