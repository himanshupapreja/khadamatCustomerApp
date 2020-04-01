using LiteDB;
using LiteDB.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiteDB.Common
{
    public sealed class LiteDBService
    {
        LiteDatabase LiteDBs;

        LiteDBService()
        {
            LiteDBs = new LiteDatabase(DataBaseAccess.DatabasePath());
        }

        private static readonly object padlock = new object();
        private static LiteDBService instance = null;
        public static LiteDBService Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new LiteDBService();
                    }
                    return instance;
                }
            }
        }

        private LiteCollection<T> GetCollection<T>()
        {
            return LiteDBs.GetCollection<T>();
        }

        public T CreateItem<T>(T item)
        {
            var collection = GetCollection<T>();
            var val = collection.Insert(item);
            return item;
        }

        public T UpdateItem<T>(BsonValue id, T item)
        {
            var collection = GetCollection<T>();
            collection.Update(id, item);
            return item;
        }

        public T DeleteItem<T>(BsonValue id, T item)
        {
            var collection = GetCollection<T>();
            var c = collection.Delete(id);
            return item;
        }

        public IEnumerable<T> ReadAllItems<T>()
        {
            var collection = GetCollection<T>();
            var all = collection.FindAll();
            return new List<T>(all);
        }
    }
}
