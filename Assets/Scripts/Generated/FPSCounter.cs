using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;

namespace Example
{
    public class FPSCounter : MonoBehaviour, INotifyPropertyChanged
    {
        public static class Events
        {
            public static Action ToggleFPS;
        }
        private float[] _fpsBuffer = new float[50]; // Buffer for averaging
        private int _fpsBufferIndex = 0;
        private string _averageFPS = "000";
        private float _updateFrequency = 1f;
        private float _nextUpdate;
        private PropertyChangedEventArgs _changedArgs;

        [SerializeField] private UIDocument uiDocument;

        private VisualElement _fpsContainer;
        private StyleEnum<DisplayStyle> _fpsContainerDisplayStyle;

        [SerializeField]
        public string AverageFPS
        {
            get => _averageFPS;
            private set 
            {
                if (_averageFPS != value)
                {
                    _averageFPS = value;
                    PropertyChanged?.Invoke(this, _changedArgs);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void Start()
        {
            _changedArgs = new PropertyChangedEventArgs(nameof(AverageFPS));
            uiDocument.rootVisualElement.dataSource = this;
            _nextUpdate = Time.time + _updateFrequency;
            _fpsContainer = uiDocument.rootVisualElement.Q("FPSContainer");
            _fpsContainerDisplayStyle = _fpsContainer.style.display;
        }

        void OnEnable()
        {
            Events.ToggleFPS += ToggleFPS;
        }

        void OnDisable()
        {
            Events.ToggleFPS -= ToggleFPS;
        }

        void Update()
        {
            if (_fpsContainer.enabledSelf)
            {
                _fpsBuffer[_fpsBufferIndex] = 1.0f / Time.deltaTime;
                _fpsBufferIndex = (_fpsBufferIndex + 1) % _fpsBuffer.Length;

                if (Time.time > _nextUpdate)
                {
                    _nextUpdate = Time.time + _updateFrequency;
                    float sum = 0;
                    foreach (float fps in _fpsBuffer)
                        sum += fps;
                    AverageFPS = $"{(int)(sum / _fpsBuffer.Length)}";
                }
            }
        }

        private void ToggleFPS()
        {
            _fpsContainer.SetEnabled(!_fpsContainer.enabledSelf);
            _fpsContainer.style.display = _fpsContainer.enabledSelf ? _fpsContainerDisplayStyle : DisplayStyle.None;
        }
    }
}
