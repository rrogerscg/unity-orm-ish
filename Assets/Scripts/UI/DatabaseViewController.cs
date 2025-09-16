using System.Collections.Generic;
using ORMish;
using UnityEngine;
using UnityEngine.UIElements;

namespace ORMIsh
{
    public class DatabaseViewController : MonoBehaviour
    {

        private static DatabaseViewController _instance;
        public static DatabaseViewController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<DatabaseViewController>();
                }
                return _instance;
            }
        }

        [SerializeField]
        private GameObject _databaseView;
        private VisualElement _ui;

        private DropdownField _tableDropdown;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            _ui = _databaseView.GetComponent<UIDocument>().rootVisualElement;
            if (_ui == null)
            {
                Debug.LogError("rootVisualElement no found on UIDocument");
            }
        }

        private void OnEnable()
        {
            _tableDropdown = _ui.Q<DropdownField>("TableDropdown");
            if (_tableDropdown == null)
            {
                Debug.LogError("TableDropdown not hooked up to controller");
            }
            else
            {
                Debug.Log("TableDropdown hooked up to controller.");
                _tableDropdown.choices = new List<string> { "TEST1", "TEST2", "TEST3" };
                _tableDropdown.value = _tableDropdown.choices[0];
            }
        }
    }

}
