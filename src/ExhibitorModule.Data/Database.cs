using System;
using System.Collections.Generic;
using ExhibitorModule.Common.Abstractions;
using ExhibitorModule.Data.Abstractions;
using SQLite;

namespace ExhibitorModule.Data
{
    public class Database : IDatabase
    {
        internal SQLiteConnection _databaseConnection;
        private readonly object _locker = new object();

        public Database(IFileService sqLite)
        {
            _databaseConnection = new SQLiteConnection(sqLite.GetCacheFilePath());
        }

        public int CreateTable<T>()
        {
            lock (_locker)
            {
                return CreateTable<T>(CreateFlags.None);
            }
        }

        public int CreateTable<T>(CreateFlags flags)
        {
            lock (_locker)
            {
                return (int)_databaseConnection.CreateTable<T>(flags);
            }
        }

        public List<T> All<T>() where T : new()
        {
            lock (_locker)
            {
                return _databaseConnection.Table<T>().ToList();
            }
        }

        public T FindWithQuery<T>(string query, params object[] args) where T : new()
        {
            lock (_locker)
            {
                return _databaseConnection.FindWithQuery<T>(query, args);
            }
        }

        public List<T> Query<T>(string query, params object[] args) where T : new()
        {
            lock (_locker)
            {
                return _databaseConnection.Query<T>(query, args);
            }
        }

        public int DropTable<T>()
        {
            lock (_locker)
            {
                return _databaseConnection.DropTable<T>();
            }
        }

        public int Insert(object obj, Type objType)
        {
            lock (_locker)
            {
                return _databaseConnection.Insert(obj, objType);
            }
        }

        public int Delete(object objectToDelete)
        {
            lock (_locker)
            {
                return _databaseConnection.Delete(objectToDelete);
            }
        }

        public int Update(object obj, Type objType)
        {
            lock (_locker)
            {
                return _databaseConnection.Update(obj, objType);
            }
        }
    }
}
