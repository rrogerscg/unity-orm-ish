using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static ORMish.SceneState;


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
            DontDestroyOnLoad(gameObject);        }

        private void Start()
        {
            PersistenceManager.Instance.Initialize();
            PersistenceManager.Instance.LoadUserCharacters();
            if (PersistenceManager.Instance.UserCharactersExist == false)
            {
                //UserCharacter newChar = new UserCharacter("Russ", "Blonde", "White", "Blue");
                //newChar.Put();
                //PersistenceManager.Instance.Save();
                //UserCharacter nc = new UserCharacter("Jett", "Blonde", "White", "Blue");
                //nc.Put();
                //UserCharacter.Table.Save();
                //_selectCharacterButton.GetComponent<Button>().interactable = false;
            }
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