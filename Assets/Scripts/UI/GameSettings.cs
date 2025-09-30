using UnityEngine;
using UnityEngine.UI;

namespace Example
{
    public class GameSettings : MonoBehaviour
    {
        [SerializeField] private Toggle _fpsToggle;
        [SerializeField] private FPSCounter _fpsCounter;

        private void OnEnable()
        {
            _fpsToggle.SetIsOnWithoutNotify(_fpsCounter.ShouldDisplayFPS);
        }

        public void ToggleFPS()
        {
            _fpsCounter.ToggleFPS();
        }
    }
}
