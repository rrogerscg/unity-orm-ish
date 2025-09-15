using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ORMish
{
    [Serializable]
    public abstract class Record<T> : IRecord where T : Record<T>, new()
    {

        public static ITable<T> Table;
        // Instance Fields
        [SerializeField]
        private readonly Guid _id;
        [SerializeField]
        private readonly DateTime _creationDate;

        public Guid Id
        {
            get => _id;
        }

        public DateTime CreationDate => _creationDate;

        public Record()
        {
            _id = Guid.NewGuid();
            _creationDate = DateTime.Now;
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
                Debug.Log("The dictionary is empty.");
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
    }
}
