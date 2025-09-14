using System;
using System.Collections.Generic;
using UnityEngine;
using static ORMish.UnlockableScriptableObject;

namespace ORMish
{
    // UnlockablesManager is an objectPooler...
    public class UnlockablesManager : MonoBehaviour
    {
        private static UnlockablesManager _instance;

        public static UnlockablesManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<UnlockablesManager>();
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
            LoadUnlockables();
            DontDestroyOnLoad(gameObject);
        }

        private Dictionary<EUnlockableType, List<GameObject>> _unlockablesByType;
        public Dictionary<EUnlockableType, List<GameObject>> UnlockablesByType => _unlockablesByType;

        public void LoadUnlockables()
        {
            Debug.Log("Loading Unlockables");
            _unlockablesByType = new();
            UnlockableScriptableObject[] unlockables = Resources.LoadAll<UnlockableScriptableObject>("");
            Debug.Log($"Total Unlockables Found: {unlockables.Length}");
            foreach (EUnlockableType unlockableType in Enum.GetValues(typeof(EUnlockableType)))
            {
                _unlockablesByType[unlockableType] = new List<GameObject>();
                foreach (UnlockableScriptableObject unlockable in unlockables)
                {
                    if (unlockable.UnlockableType == unlockableType)
                    {
                        Debug.Log("Unlockable: " + unlockable.Name + " IsUnlocked: " + unlockable.IsUnlocked);
                        GameObject unlockableGameObject = Instantiate(unlockable.Prefab, gameObject.transform);
                        unlockableGameObject.GetComponent<Unlockable>().Initialize(unlockable);
                        unlockableGameObject.SetActive(false);
                        _unlockablesByType[unlockableType].Add(unlockableGameObject);
                    }
                }
            }
            Debug.Log("Unlockables Loaded");
        }

        // SceneState will pass in GameName to filter the unlockables by game
        public void GetUnlockablePrefabsByUnlockableType(EUnlockableType unlockableType, SceneState.EGameNames gameName, Dictionary<bool, List<GameObject>> unlockables)
        {
            unlockables.Clear();
            unlockables.Add(true, new List<GameObject>());
            unlockables.Add(false, new List<GameObject>());
            foreach (GameObject unlockableGameObject in _unlockablesByType[unlockableType])
            {
                UnlockableScriptableObject unlocklable = unlockableGameObject.GetComponent<Unlockable>().UnlockableScriptableObject;
                Debug.Log($"Unlockable GameName: {gameName} GameName: {gameName}");
                if (unlocklable.GameName == gameName)
                {
                    // the key is the bool value of IsUnlocked, and the value is a list of GameObjects
                    unlockables[unlocklable.IsUnlocked].Add(unlockableGameObject);
                    Debug.Log($"Dictionary Key {unlocklable.IsUnlocked} added prefab to value: {unlockableGameObject.name}");
                }
            }
        }

        public GameObject GetUnlockableOverlay()
        {
            return Resources.Load<GameObject>("Prefabs/LockedOverlay");
        }
    }
}
