using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ORMish
{
    public abstract class GameManagerBase : MonoBehaviour
    {
        private static GameManagerBase _instance;

        public static GameManagerBase Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<GameManagerBase>();
                    if (_instance == null)
                    {
                        Debug.LogError("No instance of LevelManagerBase found in the scene.");
                    }
                }
                return _instance;
            }
        }

        [SerializeField]
        protected List<GameDataScriptableObject> _gameDataList;

        // Game LifeCycle flags
        protected bool LevelIsComplete = false;
        private bool _levelCompleteStarted = false;
        protected bool _shouldCelebrateWin;


        // Phases for the game.  Each phase will call setup with added cards
        protected int _phase;
        protected int _maxPhases;

        // initialization that does not depend on other scripts

        // This method can be called in Awake() of the derived classes.
        private void Awake()
        {
            if (_instance != null)
            {
                Debug.LogWarning($"A singleton with type '{GetType()}' is already registered!");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log($"Loaded GameData count: {_gameDataList.Count}");
                Awake_LoadPrefabs();
                Awake_InstantiateObjectsFromPrefabs();
            }
        }


        // initialization that depends on other scripts
        protected virtual void OnEnable()
        {
            // override Initialize in derived class if we want to alter phase or maxPhases
            _phase = 0;
            _maxPhases = 1;
            _shouldCelebrateWin = true;
        }


        // only called once, after OnEnable
        private void Start()
        {
            GameSetup();
        }

        // Method to call when setting up the game - should allow to reset the game when phase increases
        protected abstract void GameSetup();

        public void OnLevelActivate()
        {
            Debug.Log("OnLevelActivate");
            // Find all game objects in the scene
            GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

            // Loop through each object and set it inactive
            foreach (GameObject obj in allObjects)
            {
                obj.SetActive(true);
            }
        }

        protected abstract void Awake_InstantiateObjectsFromPrefabs(); // from LoadResources
        protected abstract void Awake_LoadPrefabs(); // from LoadResources

        // if the level is complete, then we need to set LevelIsComplete to true in this method
        protected abstract bool CheckWinCondition();

        private void Update()
        {
            if (_levelCompleteStarted)
            {
                return;
            }
            if (LevelIsComplete)
            {
                HandleLevelCompletion();
            }
            else
            {
                HandlePhaseProgression();
            }
        }

        private void HandleLevelCompletion()
        {
            _levelCompleteStarted = true;
            StartCoroutine(LevelComplete());
        }

        private void HandlePhaseProgression()
        {
            if (CheckWinCondition())
            {
                _phase++;
                if (_phase < _maxPhases)
                {
                    GameSetup();
                }
                else
                {
                    LevelIsComplete = true;
                }
            }
        }


        protected virtual IEnumerator LevelComplete()
        {
            if (_shouldCelebrateWin)
            {
                float duration = 10f;
                float elapsedTime = 0.0f;
                GameObject prefab = Resources.Load<GameObject>("Prefabs/BalloonPop/BalloonPopOverlay");
                GameObject overlay = Instantiate(prefab);
                overlay.SetActive(true);
                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }
            SceneStateManager sceneStateManager = FindAnyObjectByType<SceneStateManager>();
            SceneState nextScene = FindAnyObjectByType<SceneState>();
            sceneStateManager.ChangeState(nextScene);
        }
    }
}
