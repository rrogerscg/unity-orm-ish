using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ORMish
{
    public class ColorManager : MonoBehaviour
    {
        public SO_Colors so_colors;
        public TextMeshProUGUI colorTMP;
        public VerticalLayoutGroup verticalLayoutGroup;
        public CharacterCreationManager characterCreationManager;
        private ColorHash selectedColorHash;
        private int columns = 5;

        private void Start()
        {
            LayoutButtons();
        }

        private void CreateColorButton(ColorHash colorHash, Transform layoutTransform)
        {
            Button colorButton = Instantiate(so_colors.colorButtonPrefab, layoutTransform);
            var colors = colorButton.colors;
            colors.normalColor = colorHash.colorValue;
            colors.selectedColor = colorHash.colorValue;
            colorButton.colors = colors;
            LayoutElement buttonLayoutElement = colorButton.GetComponent<LayoutElement>();
            buttonLayoutElement.minWidth = (((RectTransform)transform).rect.width / columns);
            buttonLayoutElement.minHeight = 70f;
            colorButton.onClick.AddListener(() =>
            {
                OnColorSelect(colorHash);
            });

        }


        protected virtual void OnColorSelect(ColorHash colorHash)
        {
            selectedColorHash = colorHash;
            colorTMP.text = colorHash.colorName;
            //colorTMP.text = colorHash.colorName;
            //characterCreationManager.SetBodyColor(colorHash.colorValue);
            //override me
        }

        public ColorHash getSelectedColorHash()
        {
            return selectedColorHash;
        }

        private void LayoutButtons()
        {
            List<List<ColorHash>> colorHashRows = ListSplit(so_colors.colors, columns);
            foreach (List<ColorHash> colorHashRow in colorHashRows)
            {
                CreateButtonRow(colorHashRow);
            }
        }

        private void CreateButtonRow(List<ColorHash> colorHashes)
        {
            GameObject row = new GameObject("Row");
            row.transform.SetParent(verticalLayoutGroup.transform);
            row.transform.localScale = Vector3.one;
            row.transform.localPosition = Vector3.zero;
            row.transform.localRotation = Quaternion.identity;
            HorizontalLayoutGroup layoutGroup = row.AddComponent<HorizontalLayoutGroup>();
            layoutGroup.childForceExpandWidth = false;
            layoutGroup.childForceExpandHeight = false;
            layoutGroup.childControlWidth = true;
            layoutGroup.childControlHeight = true;
            //verticalLayoutGroup.
            foreach (ColorHash colorHash in colorHashes)
            {
                CreateColorButton(colorHash, layoutGroup.transform);
            }
        }


        private List<List<T>> ListSplit<T>(List<T> originalList, int chunkSize)
        {
            List<List<T>> result = new List<List<T>>();
            for (int i = 0; i < originalList.Count; i += chunkSize)
            {
                List<T> chunk = originalList.GetRange(i, Mathf.Min(chunkSize, originalList.Count - i));
                result.Add(chunk);
            }
            return result;
        }
    }
}
