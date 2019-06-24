using System.Collections.Generic;
using System;
using ExhibitorModule.Data.Abstractions;
using ExhibitorModule.Models;

namespace ExhibitorModule.Data
{
    public class CacheRepository : ICacheRepository
    {
        readonly IDatabase _database;
        readonly static object pass = new object();
        const string QUERY = "SELECT {0} FROM {1} WHERE {2} = ?";

        public CacheRepository(IDatabase database)
        {
            _database = database;

            lock (pass)
            {
                _database.CreateTable<CacheEntry>();
            }
        }

        public int AddOrUpdate(CacheEntry entry)
        {
            lock (pass) {
                
                entry.CreatedAt = DateTime.UtcNow;

                if(Exists(entry.Key)){
                    return _database.Update(entry, typeof(CacheEntry));
                }
                return _database.Insert(entry, typeof(CacheEntry));
            }
        }

        public bool Exists(string key)
        {
            lock (pass) {
                var query = string.Format(QUERY, "*", nameof(CacheEntry), nameof(CacheEntry.Key));
                var obj = _database.FindWithQuery<CacheEntry>(query, new[] { key });
                return obj != null && obj.Key != null && !IsExpired(obj);
            }
        }

        public CacheEntry Get(string key)
        {
            lock (pass) {
                var query = string.Format(QUERY, "*", nameof(CacheEntry), nameof(CacheEntry.Key));
                return _database.FindWithQuery<CacheEntry>(query, new[] { key });
            }
        }

        public IList<CacheEntry> GetAll()
        {
            lock (pass) {
                return _database.All<CacheEntry>();
            }
        }

        internal bool IsExpired(CacheEntry entry) {
            lock (pass)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine($"Cache {entry.Key} expires {entry.ExpiresIn.ToString()} after creation.");
#endif
                if (entry.ExpiresIn == null)
                    return false;
                
                var isExpired = DateTime.UtcNow > (entry.CreatedAt.Add(entry.ExpiresIn.Value));

                if(isExpired) {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"Cache '{entry.Key}' expired {entry.ExpiresIn.ToString()} after creation. Removing.");
#endif
                    Remove(entry);
                } else {

#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"Returning Cache '{entry.Key}'.");
#endif
                }

                return isExpired;
            }
        }

        internal bool IsExpired(string key)
        {
            lock (pass)
            {
                var entry = Get(key);
                var isExpired = DateTime.UtcNow > (entry.CreatedAt.Add(entry.ExpiresIn.Value));

                if (isExpired)
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"Cache '{entry.Key}' expired {entry.ExpiresIn.ToString()} after creation. Removing.");
#endif
                    Remove(entry);
                }

                return isExpired;
            }
        }

        public void Remove(params CacheEntry[] entries)
        {
            lock (pass)
            {
                foreach (var entry in entries)
                {
                    _database.Delete(entry);
                }
            }
        }

        public void Remove(params string[] keys)
        {
            lock (pass) {
                foreach (var key in keys)
                {
                    _database.Delete(Get(key));
                }
            }
        }

        public void Clear() {
            lock (pass) {
                var all = _database.All<CacheEntry>();
                var keys = new string[all.Count];

                for (int i = 0; i < all.Count; i++)
                {
                    keys[i] = all[i].Key;
                }
                Remove(keys);
            }
        }

        public void Destroy()
        {
            lock (pass)
            {
                _database.DropTable<CacheEntry>();
            }
        }
    }
}