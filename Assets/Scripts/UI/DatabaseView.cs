using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using ORMish;

namespace Example
{
    public class DatabaseView : MonoBehaviour
    {
        [SerializeField]
        private DatabaseStatsSO _dataStats;

        private VisualElement _ui;
        private DropdownField _tableDropdown;
        private MultiColumnListView _listView;
        private ObservableCollection<IRecord> _dataSource;

        // Store column configurations with their valueGetters
        private List<ColumnConfig> _columnConfigs = new();

        // Helper class to store column configuration including valueGetter
        private class ColumnConfig
        {
            public string name;
            public string title;
            public float width;
            public Func<IRecord, object> valueGetter;
        }

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

            // Initialize ListView settings
            _listView.fixedItemHeight = 40;
            _listView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;

            _dataStats.TableNames = DatabaseManager.Instance.GetAllTableNames();
            string names = string.Join(",", _dataStats.TableNames);
            Debug.Log($"Found the following table names from database manager '{names}'");
            _tableDropdown.RegisterValueChangedCallback(evt =>
            {
                Debug.Log($"Table Selected: {evt.newValue}");
                LoadTable(evt.newValue);
            });
        }

        private void LoadTable(string tableName)
        {
            // Clear existing columns and configurations
            _listView.columns.Clear();
            _columnConfigs.Clear();

            // Unsubscribe from previous data source if any
            if (_dataSource != null)
            {
                _dataSource.CollectionChanged -= OnDataSourceChanged;
            }

            // Get the new table
            ITable table = TableRegistry.Instance.TablesByTableName[tableName];
            Debug.Log($"Grabbing the table '{tableName}' to populate the DatabaseView");
            _dataSource = table.RecordsObservable;
            Debug.Log($"Found {_dataSource.Count} records from database");


            // Configure columns based on the first record (if available)
            if (_dataSource.Count > 0)
            {
                ConfigureColumns(_dataSource.FirstOrDefault());
            }
            else
            {
                // If no records exist, try to get column info from table type
                ConfigureColumnsFromType(table);
            }

            // Create actual ListView columns from our configurations
            CreateListViewColumns();

            // Subscribe to data changes
            _dataSource.CollectionChanged += OnDataSourceChanged;

            // Set the data source
            _listView.itemsSource = _dataSource;
            _listView.RefreshItems();
        }

        private void ConfigureColumns(IRecord record)
        {
            if (record == null) return;

            Dictionary<string, PropertyInfo> properties = GetObjectProperties(record);

            foreach (var kvp in properties)
            {
                string fieldName = kvp.Key;
                PropertyInfo propertyInfo = kvp.Value;

                // Create a valueGetter using reflection for this property
                Func<IRecord, object> valueGetter = (item) =>
                {
                    try
                    {
                        if (item != null && propertyInfo.CanRead)
                        {
                            return propertyInfo.GetValue(item);
                        }
                        return null;
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error getting value for {fieldName}: {ex.Message}");
                        return "<Error>";
                    }
                };

                ColumnConfig columnConfig = new ColumnConfig
                {
                    name = fieldName.ToLower(),
                    title = fieldName,
                    width = 150,
                    valueGetter = valueGetter
                };

                _columnConfigs.Add(columnConfig);
            }
        }

        private void ConfigureColumnsFromType(ITable table)
        {
            // Fallback method when no records exist yet
            // You might need to adjust this based on how your ITable interface works
            Type recordType = table.GetType().GetGenericArguments().FirstOrDefault();
            if (recordType != null)
            {
                PropertyInfo[] properties = recordType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo property in properties)
                {
                    if (property.CanRead)
                    {
                        string fieldName = property.Name;

                        Func<IRecord, object> valueGetter = (item) =>
                        {
                            try
                            {
                                return item != null ? property.GetValue(item) : null;
                            }
                            catch (Exception ex)
                            {
                                Debug.LogError($"Error getting value for {fieldName}: {ex.Message}");
                                return "<Error>";
                            }
                        };

                        ColumnConfig columnConfig = new ColumnConfig
                        {
                            name = fieldName.ToLower(),
                            title = fieldName,
                            width = 150,
                            valueGetter = valueGetter
                        };

                        _columnConfigs.Add(columnConfig);
                    }
                }
            }
        }

        private void CreateListViewColumns()
        {
            foreach (ColumnConfig config in _columnConfigs)
            {
                Column column = new Column
                {
                    name = config.name,
                    title = config.title,
                    width = config.width,
                    resizable = true,
                    stretchable = true,
                    makeCell = () => CreateCell(),
                    bindCell = (element, index) => BindCell(element, index, config.valueGetter)
                };

                _listView.columns.Add(column);
            }
        }

        private VisualElement CreateCell()
        {
            Label label = new();
            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            label.style.paddingLeft = 5;
            label.style.paddingRight = 5;
            return label;
        }

        private void BindCell(VisualElement element, int index, Func<IRecord, object> valueGetter)
        {
            if (_dataSource != null && index < _dataSource.Count)
            {
                Label label = element as Label;
                if (label != null)
                {
                    var value = valueGetter(_dataSource[index]);
                    label.text = value?.ToString() ?? "";
                }
            }
        }

        private void OnDataSourceChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Debug.Log($"Added {e.NewItems.Count} record(s) to the table");
                    _listView.RefreshItems();
                    break;

                case NotifyCollectionChangedAction.Remove:
                    Debug.Log($"Removed {e.OldItems.Count} record(s) from the table");
                    _listView.RefreshItems();
                    break;

                case NotifyCollectionChangedAction.Replace:
                    Debug.Log("Replaced record(s) in the table");
                    _listView.RefreshItems();
                    break;

                case NotifyCollectionChangedAction.Move:
                    Debug.Log("Moved record(s) in the table");
                    _listView.RefreshItems();
                    break;

                case NotifyCollectionChangedAction.Reset:
                    Debug.Log("Table was cleared or reset");
                    _listView.RefreshItems();
                    break;
            }
        }

        public static Dictionary<string, PropertyInfo> GetObjectProperties(object obj)
        {
            Dictionary<string, PropertyInfo> properties = new Dictionary<string, PropertyInfo>();

            if (obj == null)
            {
                Debug.Log("Object is null");
                return properties;
            }

            Type type = obj.GetType();
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in propertyInfos)
            {
                if (property.CanRead)
                {
                    properties.Add(property.Name, property);

                    try
                    {
                        object value = property.GetValue(obj);
                        Debug.Log($"  {property.Name}: {value ?? "null"}");
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"  {property.Name}: <Error: {ex.Message}>");
                    }
                }
            }

            return properties;
        }

        private void OnDestroy()
        {
            if (_dataSource != null)
            {
                _dataSource.CollectionChanged -= OnDataSourceChanged;
            }
        }
    }
}