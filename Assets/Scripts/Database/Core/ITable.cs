using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ORMish
{
    public interface ITable
    {
        string Name { get; }
        string TableFilePath { get; }
        void Initialize();
        void DeleteAllRecords();
        void Save();

        ObservableCollection<IRecord> RecordsObservable { get; }
    }
    public interface ITable<IRecord> : ITable
    {
        void PutRecord(IRecord record);
        void DeleteRecord(IRecord record);
        Dictionary<Guid, IRecord> Records { get; }
    }
}