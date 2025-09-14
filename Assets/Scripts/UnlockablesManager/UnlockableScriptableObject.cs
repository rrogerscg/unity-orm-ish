using UnityEngine;


namespace ORMish
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Unlockable", order = 1)]
    public class UnlockableScriptableObject : ScriptableObject
    {
        [SerializeField]
        private GameObject _prefab;
        public GameObject Prefab => _prefab;

        private GameObject _lockedOverlay;
        public GameObject LockedOverlay => _lockedOverlay;

        [SerializeField]
        private bool _isUnlocked;
        public bool IsUnlocked => _isUnlocked;

        public SceneState.EGameNames GameName;

        public enum EUnlockableType
        {
            FishPaintable,
            Decoration,
            Food,
            Theme
        }

        [SerializeField]
        public EUnlockableType UnlockableType;

        public enum ETheme
        {
            None,
            Farm,
            Ocean,
            Space,
            Woodland,
            Jurassic,
            Safari,
            Trundra,
            Asian,
            AquariumFree,
            AquariumPremium
        }
        [SerializeField]
        public ETheme Theme;

        public string Name => $"{UnlockableType}_{GameName.ToString()}_{Prefab.name}";
    }
}
