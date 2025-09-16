using ORMish;
using UnityEngine;
using UnityEngine.UIElements;

namespace ORMish
{
    public class DatabaseView : MonoBehaviour
    {
        [SerializeField]
        private DatabaseStatsSO _dataStats;

        private VisualElement _ui;

        private DropdownField _tableDropdown;

        // Setting the ui element in OnEnable seems to work, vs Awake or Start
        private void OnEnable()
        {
            _ui = GetComponent<UIDocument>().rootVisualElement;
            if (_ui == null)
            {
                Debug.LogError("rootVisualElement no found on UIDocument");
            }
            else
            {
                Debug.Log($"_ui set to: {_ui}");
            }
            _tableDropdown = _ui.Q<DropdownField>("TableDropdown");
            if (_tableDropdown == null)
            {
                Debug.LogError("TableDropdown not hooked up to controller");
            }
            else
            {
                Debug.Log("TableDropdown hooked up to controller.");
            }
            _dataStats.TableNames = DatabaseManager.Instance.GetAllTableNames();
            _tableDropdown.RegisterValueChangedCallback(evt =>
            {
                Debug.Log($"New Table Selected: {evt.newValue}");
                Debug.Log("Listing records");
                ITable table = TableRegistry.TablesByTableName[evt.newValue];
                foreach(IRecord record in table.GetRecords())
                {
                    Debug.Log(record);
                }
            });
        }
    }

}
