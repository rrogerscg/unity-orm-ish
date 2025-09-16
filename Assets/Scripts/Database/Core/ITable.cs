using System;
using System.Collections.Generic;

namespace ORMish
{
    public interface ITable
    {
        string Name { get; }
        string TableFilePath { get; }
        void Initialize();
        void DeleteAllRecords();
        void Save();

        List<IRecord> GetRecords();

    }
    public interface ITable<T> : ITable
    {
        void PutRecord(T record);
        void DeleteRecord(T record);
        Dictionary<Guid, T> Records { get; }
    }
}