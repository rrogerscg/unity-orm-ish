using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
    [SerializeField]
    private GameObject _uiDocGameObject;

    private void Start()
    {
        _uiDocGameObject.SetActive(false);
    }

    public void Toggle()
    {
        if(_uiDocGameObject != null)
        {
            _uiDocGameObject.SetActive(!_uiDocGameObject.activeSelf);
        }
    }
}
