using UnityEngine;

namespace Example
{
    public class FSMachineSingleton<T> : MonoBehaviour where T : FSMachineSingleton<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();
                    DontDestroyOnLoad(_instance);

                }
                return _instance;
            }
        }

        protected IState _currentState;
        public IState CurrentState => _currentState;

        protected IState _previousState;
        public IState PreviousState => _previousState;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = (T)this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        public virtual void ChangeState(IState newState)
        {
            if (_currentState != null && _currentState.Name != newState.Name)
            {
                _currentState.Exit();
            }
            _previousState = _currentState;
            _currentState = newState;
            _currentState.Enter();
        }
    }

}
