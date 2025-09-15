using UnityEngine;

public class ToggleCanvas : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;

    private void Start()
    {
        _canvas.gameObject.SetActive(false);
    }

    public void Toggle()
    {
        _canvas.gameObject.SetActive(!_canvas.gameObject.activeSelf);
    }
}
