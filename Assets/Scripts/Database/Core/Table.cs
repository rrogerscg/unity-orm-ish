using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace ORMish
{
    public class Table<TRecord> : ITable<TRecord> where TRecord : Record<TRecord>, new()
    {
        private Dictionary<Guid, TRecord> _records = new();
        public Dictionary<Guid, TRecord> Records => _records;
        public string Name => typeof(TRecord).Name + "s";
        private readonly string _tableFilePath;
        public string TableFilePath => _tableFilePath;

        public Table(string tableDirectory)
        {
            if (!Directory.Exists(tableDirectory))
            {
                Directory.CreateDirectory(tableDirectory);
            }
            _tableFilePath = Path.Combine(tableDirectory, Name + ".json");
            if (!File.Exists(_tableFilePath))
            {
                File.Create(_tableFilePath).Close();
            }
            TableRegistry.Register(this);
            Initialize();
        }

        public void Initialize()
        {
            _records.Clear();
            _records = LoadRecords();
        }

        public IEnumerable<IRecord> GetRecords()
        {
            return _records.Values.Cast<IRecord>();
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
                }
                else
                {
                    Debug.Log("No records found for type: " + Name);
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
            Debug.Log("PutRecord for type: " + Name);
            if (_records.ContainsKey(record.Id))
            {
                _records[record.Id] = record;
            }
            else
            {
                _records.Add(record.Id, record);
            }
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(_records, Formatting.Indented);
            Debug.Log("TableFilePath");
            Debug.Log(TableFilePath);
            File.WriteAllText(TableFilePath, json);
        }
    }
}

