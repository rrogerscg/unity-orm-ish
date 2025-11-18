using System;
using System.Collections.Generic;
using System.Linq;

namespace ORMish
{
    [Serializable]
    public abstract class Record<T> : IRecord where T : Record<T>, new()
    {

        private static ITable<T> _table;
        public static ITable<T> Table => _table;
        // Instance Fields
        private readonly Guid _id;
        private readonly DateTime _creationDate;
        private readonly string _type = typeof(T).Name;

        public Guid Id
        {
            get => _id;
        }

        public DateTime CreationDate => _creationDate;

        public string Type
        {
            get => _type;
        }

        public Record()
        {
            _id = Guid.NewGuid();
            _creationDate = DateTime.Now.ToUniversalTime();
        }

        public Record(Guid id, DateTime creationDate)
        {
            _id = id;
            _creationDate = creationDate;
        }

        public virtual void Delete()
        {
            Table.DeleteRecord((T)this);
        }

        public virtual void Put()
        {
            Table.PutRecord((T)this);
        }

        public static List<T> GetAll()
        {
            if (Table.Records.Count == 0)
            {
                return new List<T>();
            }
            return Table.Records.Values.ToList();
        }

        public static void DeleteAll()
        {
            Table.DeleteAllRecords();
        }

        public static T GetById(Guid id)
        {
            return Table.Records[id];
        }

        public override bool Equals(object obj)
        {
            if (obj is Record<T> other)
            {
                return GetHashCode() == other.GetHashCode();
            }
            return false;
        }

        public abstract override int GetHashCode();

        public static void SaveTable()
        {
            Table.Save();
        }

        public static void ReloadRecords()
        {
            Table.Initialize();
        }

        public static void ConnectToTable()
        {
            Console.WriteLine($"ConnectToTable called for type: {typeof(T).Name}");
            // table file gets created on instantiating a new table
            _table = new Table<T>();
        }
    }
}
