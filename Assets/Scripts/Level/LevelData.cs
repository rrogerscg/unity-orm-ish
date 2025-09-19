namespace Example
{
    public class LevelData : ILevelData
    {
        protected string _name;
        public string Name => _name;

        public LevelData(string name)
        {
            _name = name;
        }
    }    
}
