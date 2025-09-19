using UnityEngine;
using UnityEngine.UIElements;


namespace Example
{
   public class ToggleGameObject : MonoBehaviour
    {
        [SerializeField]
        private GameObject _uiGameObject;
        private UIDocument _uiDocument;

        private void Start()
        {
            _uiGameObject.SetActive(true);
            _uiDocument = _uiGameObject.GetComponent<UIDocument>();
            _uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        }

        public void Toggle()
        {
            if (_uiDocument != null)
            {
                StyleEnum<DisplayStyle> style = _uiDocument.rootVisualElement.style.display;
                _uiDocument.rootVisualElement.style.display = style == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }
    }
}
