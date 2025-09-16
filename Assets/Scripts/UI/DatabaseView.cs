using System;
using System.Collections.Generic;
using System.Reflection;
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

        private MultiColumnListView _listView;
        private void Awake()
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

            _listView = _ui.Q<MultiColumnListView>("RecordsTable");

            _dataStats.TableNames = DatabaseManager.Instance.GetAllTableNames();
            _tableDropdown.RegisterValueChangedCallback(evt =>
            {
                Debug.Log($"New Table Selected: {evt.newValue}");
                Debug.Log("Listing records");
                ITable table = TableRegistry.TablesByTableName[evt.newValue];
                List<IRecord> records = table.GetRecords();
                _listView.itemsSource = records;
                if (records.Count > 0)
                {
                    Dictionary<string, string> firstRecordFields = GetObjectFields(records[0]);

                    foreach (KeyValuePair<string, string> kvp in firstRecordFields)
                    {
                        string fieldName = kvp.Key;

                        Column newColumn = new Column
                        {
                            name = fieldName.ToLower(),
                            title = fieldName,
                            width = 150,
                            resizable = true,
                            stretchable = true
                        };


                        newColumn.bindCell = (element, index) =>
                        {
                            Label label = element as Label ?? new Label();
                            label.AddToClassList("tableRow");
                            // Get the current record for this row
                            if (index >= 0 && index < records.Count)
                            {
                                Dictionary<string, string> currentRecordFields = GetObjectFields(records[index]);
                                label.text = currentRecordFields.ContainsKey(fieldName)
                                    ? currentRecordFields[fieldName]
                                    : "N/A";
                            }
                            else
                            {
                                label.text = "Invalid";
                            }

                            if (element != label)
                            {
                                element.Clear();
                                element.Add(label);
                            }
                        };

                        _listView.columns.Add(newColumn);
                    }
                }

                _listView.RefreshItems();
                //_multiColumnListView.itemsSource = table.GetRecords();
            });
        }

        // Setting the ui element in OnEnable seems to work, vs Awake or Start
        private void OnEnable()
        {
            
        }

        public static Dictionary<string, string> GetObjectFields(object obj)
        {
            Dictionary<string, string> fields = new();
            if (obj == null)
            {
                Debug.Log("Object is null");
            }

            Type type = obj.GetType();

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                try
                {
                    if (property.CanRead)
                    {
                        object value = property.GetValue(obj);
                        Debug.Log($"  {property.Name}: {value ?? "null"}");
                        fields.Add(property.Name, value.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"  {property.Name}: <Error: {ex.Message}>");
                }
            }
            return fields;
        }
    }

}
