namespace Example
{
    public interface IState
    {
        public string Name { get; }
        void Enter();

        void Exit();
    }
}
