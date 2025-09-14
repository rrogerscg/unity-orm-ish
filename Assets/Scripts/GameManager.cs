using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static ORMish.SceneState;
using static ORMish.UnlockableScriptableObject;


namespace ORMish
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<GameManager>();
                }
                return _instance;
            }
        }

        public Dictionary<bool, List<GameObject>> ThemeUnlockablesMap;
        public EGameNames ActiveGameName;

        private bool _userIsSubscribed;
        public bool UserIsSubscribed => _userIsSubscribed;
        public ETheme ActiveTheme;

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
        }

        private void Start()
        {
            InitializePersistenceManager();
            ThemeSelectionEvents.ThemeClicked += (isUnlockable, theme, nextSceneState) => LoadActiveGameWithTheme(isUnlockable, theme, nextSceneState);
            LoadActiveGameWithTheme(false, ETheme.AquariumFree, _startingScene);

        }

        private void LoadActiveGameWithTheme(bool isUnlockable, ETheme theme, SceneState nextSceneState)
        {
            Debug.Log("[GameManager::LoadActiveGameWithTheme]");
            // Load unlockable if user is scubscribed or if the theme is not unlockable
            if (UserIsSubscribed || !isUnlockable)
            {
                ActiveTheme = theme;
                Debug.Log($"Active theme set to {ActiveTheme}");
                SceneStateManager.Instance.ChangeState(nextSceneState);
            }
            // If user is not subscribed and the theme is unlockable, show subscription screen
            else
            {
                Debug.LogError("User is not subscribed. Show subscription screen.");
                //SceneStateManager.Instance.ChangeState(new SubscriptionState());
            }
        }

        private void InitializePersistenceManager()
        {
            PersistenceManager.Instance.Initialize();
        }

        public bool CharacterDataIsLoaded()
        {
            return PersistenceManager.Instance.ActiveUser != null;
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

        public void LoadThemesByActiveGameName()
        {
            ThemeUnlockablesMap = new();
            UnlockablesManager.Instance.GetUnlockablePrefabsByUnlockableType(EUnlockableType.Theme, ActiveGameName, ThemeUnlockablesMap);
        }

        public void GetScriptableObjectsByGame(List<GameDataScriptableObject> _gameDataList)
        {
            Debug.Log($"Loading GameData for {ActiveGameName}, {ActiveTheme}");
            GameDataManager.Instance.GetGameDataScriptableObjectsByGameNameAndTheme(ActiveGameName, ActiveTheme, _gameDataList);
        }

        private void OnExitGameAction()
        {
            if (Application.isEditor)
            {
                EditorApplication.isPlaying = false;
            }
            else
            {
                Application.Quit();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnExitGameAction();
            }
        }
    }
}