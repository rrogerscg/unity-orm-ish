using UnityEngine;
using UnityEngine.UIElements;

public class ToggleGameObject : MonoBehaviour
{
    [SerializeField]
    private UIDocument _uiDocument;

    private void Start()
    {
        _uiDocument.rootVisualElement.style.display = DisplayStyle.None;
    }

    public void Toggle()
    {
        if(_uiDocument != null)
        {
            StyleEnum<DisplayStyle> style = _uiDocument.rootVisualElement.style.display;
            _uiDocument.rootVisualElement.style.display = style == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
