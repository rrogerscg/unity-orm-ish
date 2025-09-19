using System.Collections.Generic;
using UnityEngine;
using System;


namespace Example
{
    public class FSMachine<EState> : MonoBehaviour where EState : Enum
    {
        protected Dictionary<EState, BaseState<EState>> _states = new();


        protected BaseState<EState> _currentState;
        public BaseState<EState> CurrentState => _currentState;

        protected BaseState<EState> _previousState;
        public BaseState<EState> PreviousState => _previousState;

        protected bool _isTransitioningState = false;

        public virtual void Awake()
        {
            _currentState = null;
            _previousState = null;
        }

        public virtual void Start()
        {

        }

        protected virtual void Update()
        {
            EState nextStateKey = CurrentState.GetNextState();
            if (!_isTransitioningState)
            {
                if (nextStateKey.Equals(CurrentState.StateKey))
                {
                    CurrentState.UpdateState();
                }
                else
                {
                    Debug.Log($"Should transition to scene: {nextStateKey}");
                    TransitionToState(nextStateKey);
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            _currentState?.FixedUpdateState();
        }

        protected void ActivatePreviousState()
        {
            if(_previousState != null)
            {
                TransitionToState(_previousState.StateKey);
            }
        }

        protected virtual void TransitionToState(EState nextStateKey)
        {
            if (_states.ContainsKey(nextStateKey))
            {
                _isTransitioningState = true;
                _previousState = _currentState;
                _currentState.ExitState();
                _currentState = _states[nextStateKey];
                _currentState.EnterState();
                _isTransitioningState = false;
            }
            else
            {
                Debug.LogError($"State {nextStateKey} not found in state machine.");
            }
        }
    }
}

