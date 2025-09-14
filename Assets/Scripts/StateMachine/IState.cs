namespace ORMish
{
    public interface IState
    {
        public string Name { get; }
        void Enter();

        void Exit();
    }
}
