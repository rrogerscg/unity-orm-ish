using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



namespace Example
{
    public class NewCharacterCreatorController : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField _nameInputField;
        [SerializeField]
        private TMP_Dropdown _hairColorDropDown;
        [SerializeField]
        private TMP_Dropdown _eyeColorDropDown;
        [SerializeField]
        private TMP_Dropdown _skinColorDropDown;

        private List<TMP_Dropdown> _dropdowns = new();

        private void Awake()
        {
            _dropdowns.Add(_hairColorDropDown);
            _dropdowns.Add(_eyeColorDropDown);
            _dropdowns.Add(_skinColorDropDown);
        }

        private void OnEnable()
        {
            foreach(TMP_Dropdown dd in _dropdowns)
            {
                Color color = dd.options[dd.value].color;
                ColorBlock cb = dd.colors;
                cb.selectedColor = color;
                cb.normalColor = color;
                dd.colors = cb;
                dd.onValueChanged.AddListener(async (index) =>
                {
                    Color color = dd.options[index].color;
                    ColorBlock cb = dd.colors;
                    cb.selectedColor = color;
                    cb.normalColor = color;
                    dd.colors = cb;
                });
            }
        }

        public void CreateCharacterClicked()
        {
            UserCharacter character = new UserCharacter(
                _nameInputField.text,
                _hairColorDropDown.options[_hairColorDropDown.value].text,
                _eyeColorDropDown.options[_eyeColorDropDown.value].text,
                _skinColorDropDown.options[_skinColorDropDown.value].text
                );

            character.Put();
            ToastManager.ToastEvents.OnShowEvent?.Invoke($"New UserCharacter created with name: {character.Name}");
            UserCharacter.Table.Save();
            Debug.Log($"New UserCharacter created");
        }
    }
}

