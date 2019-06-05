using System;
using System.Threading.Tasks;

namespace ExhibitorModule.Services.Abstractions
{
    public interface ICacheService
    {
        IMemoryCache Memory { get; set; }
        IDeviceCache Device { get; set; }
    }

    public interface IMemoryCache : ICache { }
    public interface IDeviceCache : ICache { }

    public interface ICache : IDisposable
    {
        bool Exists(string key);
        void Remove(params string[] keys);
        void Clear();
        string GetValue(string key);
        T GetObject<T>(string key);
        byte[] GetObject(string key);
        void AddOrUpdateValue(string key, object value, TimeSpan? expiryPeriod = null);
        void AddOrUpdateValue(string key, byte[] blob, TimeSpan? expiryPeriod = null);

        /// <summary>
        /// Gets the value if available. Otherwise invoke provided func and cache the new value.
        /// </summary>
        /// <returns>Newly fetched and cached value.</returns>
        /// <param name="key">Key</param>
        /// <param name="func">Func to invoke if cache is missing or expired</param>
        /// <param name="expiryPeriod">Expiry period for new value</param>
        /// <typeparam name="T">Type of value to cache</typeparam>
        T GetOrFetch<T>(string key, Func<Task<T>> func, TimeSpan? expiryPeriod = null);
    }
}
