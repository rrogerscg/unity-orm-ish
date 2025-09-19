using System.Collections.Generic;
using UnityEngine;

namespace Example
{
    public class NavigationMenuManager : MonoBehaviour
    {

        private static NavigationMenuManager _instance;
        public static NavigationMenuManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<NavigationMenuManager>();
                }
                return _instance;
            }
        }
        [SerializeField]
        private Canvas _mainMenuCanvas;
        [SerializeField]
        private Canvas _newCharacterCanvas;
        [SerializeField]
        private Canvas _selectCharactersCanvas;
        [SerializeField]
        private Canvas _settingsCanvas;

        private Dictionary<ENavigationMenu, Canvas> _canvasByEMenu = new();

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            _canvasByEMenu.Add(ENavigationMenu.MainMenu, _mainMenuCanvas);
            _canvasByEMenu.Add(ENavigationMenu.NewCharacter, _newCharacterCanvas);
            _canvasByEMenu.Add(ENavigationMenu.SelectCharacter, _selectCharactersCanvas);
            _canvasByEMenu.Add(ENavigationMenu.Settings, _settingsCanvas);
        }

        private void OnEnable()
        {
            NavigationEvents.OnNavigateToMenu += MenuSwitch;
        }

        private void OnDisable()
        {
            NavigationEvents.OnNavigateToMenu -= MenuSwitch;
        }

        private void MenuSwitch(ENavigationMenu menu)
        {
            foreach (KeyValuePair<ENavigationMenu, Canvas> kvp in _canvasByEMenu)
            {
                kvp.Value.gameObject.SetActive(kvp.Key == menu);
            }
        }
    }
}

