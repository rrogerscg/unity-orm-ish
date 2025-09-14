using System;
using System.Collections.Generic;

namespace ORMish
{
    public interface ITable<T>
    {
        void PutRecord(T record);
        void DeleteRecord(T record);

        void Initialize();
        void DeleteAllRecords();
        void Save();
        Dictionary<Guid, T> Records { get; }
        string Name { get; }
        string TableFilePath { get; }
    }
}