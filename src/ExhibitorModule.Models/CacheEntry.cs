using System;
using SQLite;

namespace ExhibitorModule.Models
{
    public class CacheEntry
    {
        [PrimaryKey, Unique]
        public string Key { get; set; }
        public string Value { get; set; }
        public byte[] Blob { get; set; }
        [NotNull]
        public DateTime CreatedAt { get; set; }
        public TimeSpan? ExpiresIn { get; set; }
    }
}
