using System.IO;
using ExhibitorModule.Common.Abstractions;
using ExhibitorModule.Services.Abstractions;

namespace ExhibitorModule.Droid
{
    public class FileService : IFileService
    {
        public string GetCacheFilePath()
        {
            var fileName = "ExhibitorModuleDatabase.db3";
            var dirPath = Android.App.Application.Context.CacheDir.AbsolutePath;
            var path = Path.Combine(dirPath, fileName);

            object pass = new object();
            lock (pass)
            {
                if (!File.Exists(path))
                    File.Create(path);
            }
            return path;
        }

        public string GetCssFilePath()
        {
            return "file:///android_asset/";
        }
    }
}
