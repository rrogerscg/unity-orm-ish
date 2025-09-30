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

        [SerializeField] private UIDocument _uiDocument;

        private VisualElement _fpsContainer;
        private StyleEnum<DisplayStyle> _fpsContainerDisplayStyle;

        public bool ShouldDisplayFPS => _fpsContainer.enabledSelf;

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
            _uiDocument.rootVisualElement.dataSource = this;
            _nextUpdate = Time.time + _updateFrequency;
            _fpsContainer = _uiDocument.rootVisualElement.Q("FPSContainer");
            _fpsContainerDisplayStyle = _fpsContainer.style.display;
            ToggleFPS();
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
                // this is a circular buffer
                // Explanation:
                // when _fpsBufferIndex reaches 49 and added to 1 = 50
                // then we 50 modulo 50, which is 0 and sets the index back to 0
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

        public void ToggleFPS()
        {
            _fpsContainer.SetEnabled(!_fpsContainer.enabledSelf);
            _fpsContainer.style.display = _fpsContainer.enabledSelf ? _fpsContainerDisplayStyle : DisplayStyle.None;
            string state = _fpsContainer.enabledSelf ? "enabled" : "disabled";
            ToastManager.ToastEvents.OnShowEvent?.Invoke($"FPS has been {state}");
        }
    }
}
