using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

namespace ORMish
{
    public class DatabaseViewDropdown : MonoBehaviour
    {
        [SerializeField]
        private TMP_Dropdown _dropdown;

        private void OnEnable()
        {
            _dropdown.ClearOptions();

            List<TMP_Dropdown.OptionData> options = new();
            foreach (string dbName in DatabaseManager.Instance.GetAllTableNames())
            {
                options.Add(new TMP_Dropdown.OptionData(dbName));
            }
            _dropdown.AddOptions(options);
            _dropdown.RefreshShownValue();
        }
    }
}

