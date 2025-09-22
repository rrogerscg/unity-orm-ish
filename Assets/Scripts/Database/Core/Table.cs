using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;

namespace ORMish
{
    // New table JSON files get created when a derived Record object is referenced
    public class Table<TRecord> : ITable<TRecord> where TRecord : Record<TRecord>, new()
    {
        private Dictionary<Guid, TRecord> _records = new();
        public Dictionary<Guid, TRecord> Records => _records;
        public string Name => typeof(TRecord).Name;
        private readonly string _tableFilePath;
        public string TableFilePath => _tableFilePath;

        private ObservableCollection<IRecord> _recordsObservable = new();
        public ObservableCollection<IRecord> RecordsObservable => _recordsObservable;

        public Table()
        {
            if (!Directory.Exists(DatabaseManager.Instance.TablesPath))
            {
                Directory.CreateDirectory(DatabaseManager.Instance.TablesPath);
            }
            _tableFilePath = Path.Combine(DatabaseManager.Instance.TablesPath, Name + ".json");
            Console.WriteLine($"_tableFilePath: {_tableFilePath}");
            if (!File.Exists(_tableFilePath))
            {
                File.Create(_tableFilePath).Close();
            }
            Initialize();
        }

        public void Initialize()
        {
            TableRegistry.Instance.Register(this);
            _records.Clear();
            _records = LoadRecords();
        }

        public Dictionary<Guid, TRecord> LoadRecords()
        {
            Dictionary<Guid, TRecord> records = new();
            if (File.Exists(_tableFilePath))
            {
                string json = File.ReadAllText(_tableFilePath);
                if (json.Length > 0)
                {
                    records = JsonConvert.DeserializeObject<Dictionary<Guid, TRecord>>(json);
                    _recordsObservable = new ObservableCollection<IRecord>(records.Values);
                }
                else
                {
                    Console.WriteLine("No records found for type: " + Name);
                }
            }
            return records;
        }

        public void DeleteRecord(TRecord record)
        {
            if (_records.ContainsKey(record.Id))
            {
                _records.Remove(record.Id);
            }
            
        }

        public void DeleteAllRecords()
        {
            _records.Clear();
            Save();
        }


        public void PutRecord(TRecord record)
        {
            Console.WriteLine("PutRecord for type: " + Name);
            if (_records.ContainsKey(record.Id))
            {
                _records[record.Id] = record;
            }
            else
            {
                _records.Add(record.Id, record);
                _recordsObservable.Add(record);
            }
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(_records, Formatting.Indented);
            Console.WriteLine("TableFilePath");
            Console.WriteLine(TableFilePath);
            File.WriteAllText(TableFilePath, json);
        }

    }
}

