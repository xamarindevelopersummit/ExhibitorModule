using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using ExhibitorModule.Services.Abstractions;
using ExhibitorModule.Models;
using ExhibitorModule.Data.Abstractions;

namespace ExhibitorModule.Services
{
    public class CacheService : ICacheService
    {
        public IMemoryCache Memory { get; set; }
        public IDeviceCache Device { get; set; }

        public CacheService(IMemoryCache memoryCache, IDeviceCache deviceCache)
        {
            Memory = memoryCache;
            Device = deviceCache;
        }
    }

    /// <summary>
    /// Memory cache.
    /// </summary>
    public class MemoryCache : IMemoryCache
    {
        static Dictionary<string, CacheEntry> _inMemory;
        static readonly object pass = new object();

        public MemoryCache()
        {
            lock (pass)
            {
                _inMemory = new Dictionary<string, CacheEntry>();
#if DEBUG
                System.Diagnostics.Debug.WriteLine("Memory Cache initialized.");
#endif
            }
        }

        public T GetObject<T>(string key)
        {
            lock (pass)
            {
                try {
                    if (Exists(key))
                        return JsonConvert.DeserializeObject<T>(_inMemory[key].Value);
                    return default(T);
                } catch (Exception ex) {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"{(ex.InnerException??ex).Message}");
#endif
                    return default(T);
                }
            }
        }

        public byte[] GetObject(string key)
        {
            try
            {
                if (Exists(key))
                {
                    var retVal = _inMemory[key];
                    return retVal.Blob;
                }
                
                return default(byte[]);
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine($"{(ex.InnerException ?? ex).Message}");
#endif
                return default(byte[]);
            }
        }

        public void AddOrUpdateValue(string key, object value, TimeSpan? expiryPeriod = null)
        {
            lock (pass)
            {
                var ser = JsonConvert.SerializeObject(value);
                _inMemory[key] = new CacheEntry { Key = key, Value = ser, CreatedAt = DateTime.UtcNow, ExpiresIn = expiryPeriod };
#if DEBUG
                if(expiryPeriod == null)
                    System.Diagnostics.Debug.WriteLine($"Cache {key} added.");
                else
                    System.Diagnostics.Debug.WriteLine($"Cache {key} added and will expire in {expiryPeriod.ToString()}.");
#endif
            }
        }

        public void AddOrUpdateValue(string key, byte[] value, TimeSpan? expiryPeriod = null)
        {
            lock (pass)
            {
                _inMemory[key] = new CacheEntry { Key = key, Blob = value, CreatedAt = DateTime.UtcNow, ExpiresIn = expiryPeriod };
            }
        }

        public bool Exists(string key)
        {
            lock (pass)
            {
                if (!_inMemory.ContainsKey(key))
                    return false;

                var entry = _inMemory[key];
                return entry != null && !IsExpired(entry);
            }
        }

        public void Remove(params string[] keys)
        {
            lock (pass)
            {
                foreach (var key in keys)
                {
                    if(_inMemory.ContainsKey(key))
                        _inMemory.Remove(key);
                }
            }
        }

        public void Clear()
        {
            lock (pass)
            {
                _inMemory.Clear();
            }
        }

        public void Dispose() 
        {
            lock (pass)
            {
                Clear();
                _inMemory = null;
            }
        }

        public override string ToString()
        {
            var retVal = new StringBuilder();
            retVal.AppendLine($"Count: {_inMemory.Count}");
            foreach (var item in _inMemory)
            {
                retVal.AppendLine($"{item.Key} = {item.Value}");
            }
            return retVal.ToString();
        }

        public string GetValue(string key)
        {
            lock (pass) {
                return Exists(key) ? JsonConvert.DeserializeObject<string>(_inMemory[key].Value) : null;
            }
        }

        public T GetOrFetch<T>(string key, Func<Task<T>> func, TimeSpan? expiryPeriod = null)
        {
            if (Exists(key))
            {
                var entry = _inMemory[key];
                return JsonConvert.DeserializeObject<T>(entry.Value);
            }

            var tcs = new TaskCompletionSource<T>();

            Task.Run(async () => {
                try
                {
                    tcs.SetResult(await func.Invoke());
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            var retVal = (T)tcs.Task.Result;

            AddOrUpdateValue(key, retVal, expiryPeriod);
            return (T)retVal;
        }

        internal bool IsExpired(CacheEntry entry)
        {
            lock (pass)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine($"Cache {entry.Key} expires {entry.ExpiresIn.ToString()} after creation.");
#endif
                if (entry.ExpiresIn == null)
                    return false;

                var isExpired = DateTime.UtcNow > (entry.CreatedAt.Add(entry.ExpiresIn.Value));

                if (isExpired)
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"Cache '{entry.Key}' expired {entry.ExpiresIn.ToString()} after creation. Removing.");
#endif
                    Remove(entry.Key);
                }

                return isExpired;
            }
        }
    }

    /// <summary>
    /// Device cache.
    /// </summary>
    public class DeviceCache : IDeviceCache
    {
        ICacheRepository _cacheRepo;

        public DeviceCache(ICacheRepository cacheRepo)
        {
            _cacheRepo = cacheRepo;
        }

        public void AddOrUpdateValue(string key, object value, TimeSpan? expiryPeriod = null)
        {
            var ser = JsonConvert.SerializeObject(value);
            _cacheRepo.AddOrUpdate(new CacheEntry { Key = key, Value = ser, ExpiresIn = expiryPeriod });
#if DEBUG
            if(expiryPeriod == null)
                System.Diagnostics.Debug.WriteLine($"Cache {key} added");
            else
                System.Diagnostics.Debug.WriteLine($"Cache {key} added and will expire in {expiryPeriod.ToString()}.");
#endif
        }

        public void AddOrUpdateValue(string key, byte[] value, TimeSpan? expiryPeriod = null)
        {
            if (key == null) {
#if DEBUG
                System.Diagnostics.Debug.WriteLine($"Key is null. Cannot cache.");
#endif
                return; 
            }

            _cacheRepo.AddOrUpdate(new CacheEntry { Key = key, Blob = value, ExpiresIn = expiryPeriod });
#if DEBUG
            if(expiryPeriod == null)
                System.Diagnostics.Debug.WriteLine($"Cached {key}.");
            else
                System.Diagnostics.Debug.WriteLine($"Cache {key} added and will expire in {expiryPeriod.ToString()}.");
#endif
        }

        public void Clear()
        {
            _cacheRepo.Clear();
        }

        public void Dispose()
        {
            _cacheRepo = null;
        }

        public bool Exists(string key)
        {
            return _cacheRepo.Exists(key);
        }

        public string GetValue(string key)
        {
            return Exists(key) ? SanitizedString(_cacheRepo.Get(key).Value) : null;
        }

        string SanitizedString(string text)
        {
            var ret = text.TrimStart('"');
            return ret.TrimEnd('"');
        }

        public T GetObject<T>(string key)
        {
            try
            {
                if(!Exists(key)) {
                    return default(T);
                }

                var retVal = _cacheRepo.Get(key);
                return JsonConvert.DeserializeObject<T>(retVal.Value);
            }
            catch
            {
                throw new Exception("Object not found in cache.");
            }
        }

        public byte[] GetObject(string key)
        {
            return Exists(key) ? _cacheRepo.Get(key).Blob : default(byte[]);
        }

        public void Remove(params string[] keys)
        {
            foreach (var key in keys)
            {
                if (_cacheRepo.Exists(key))
                    _cacheRepo.Remove(key);
            }
        }

        public T GetOrFetch<T>(string key, Func<Task<T>> func, TimeSpan? expiryPeriod = null)
        {
            if (Exists(key))
            {
                var entry = _cacheRepo.Get(key);
                return JsonConvert.DeserializeObject<T>(entry.Value);
            }

            var tcs = new TaskCompletionSource<T>();

            Task.Run(async () => {
                try
                {
                    tcs.SetResult(await func.Invoke());
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            var retVal = (T) tcs.Task.Result;

            AddOrUpdateValue(key, retVal, expiryPeriod);
            return (T) retVal;
        }

        public override string ToString()
        {
            var items = _cacheRepo.GetAll();
            var retVal = new StringBuilder();
            retVal.AppendLine($"Count: {items.Count}");
            foreach (var item in items)
            {
                retVal.AppendLine($"{item.Key} = {item.Value}");
            }
            return retVal.ToString();
        }
    }
}
