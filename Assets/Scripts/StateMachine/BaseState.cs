using System;

namespace Example
{
    public abstract class BaseState<EState> where EState : Enum
    {
        public BaseState(EState stateKey)
        {
            StateKey = stateKey;
        }

        public EState StateKey { get; protected set; }
        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
        public abstract EState GetNextState();

        public virtual void FixedUpdateState() { }
    }
}
