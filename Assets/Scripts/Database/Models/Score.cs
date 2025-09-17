using System;

namespace ORMish
{
    [Serializable]
    public class Score : Record<Score>
    {
        public int Value { get; set; }
        public string Name { get; set; }

        // Required constructor to use for generic type
        public Score() : base()
        {

        }

        // Create a new Score object with a value and name
        public Score(int value, string name) : base()
        {
            Value = value;
            Name = name;
        }

        public override int GetHashCode()
        {

            return HashCode.Combine(Id, CreationDate, Value, Name);
        }
    }
}
