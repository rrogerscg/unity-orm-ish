using TMPro;
using UnityEngine;

namespace Example
{
    public class ToastTemplate : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private RectTransform _timer;
        private int _timerWidth;
        public int TimerWidth => _timerWidth;
        private float _duration; // set from manager
        private bool _isAlive = true;
        public bool IsAlive => _isAlive;
        private float _elapsed;

        private void Start()
        {
            _timerWidth = (int)_timer.rect.width;
        }

        private void Update()
        {
            _elapsed += Time.deltaTime;
            float _newWidth = Mathf.Lerp(0f, _timerWidth, _elapsed / _duration);
            _timer.sizeDelta = new Vector2(_newWidth, _timer.sizeDelta.y);
            if(_elapsed >= _duration)
            {
                _isAlive = false;
            }
        }

        public void StartToast(string text, float duration)
        {
            _text.text = text;
            _duration = duration;
            _isAlive = true;
            _elapsed = 0;
            transform.SetAsLastSibling();
        }
    }
}

