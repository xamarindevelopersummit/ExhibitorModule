using System.IO;
using ExhibitorModule.Common.Abstractions;
using Foundation;

namespace ExhibitorModule.iOS
{
    public class FileService : IFileService
    {
        public string GetCssFilePath()
        {
            return NSBundle.MainBundle.BundlePath;
        }

        public string GetCacheFilePath()
        {
            var fileName = "ExhibitorModuleDatabase.db3";
            var dirPath = NSSearchPath.GetDirectories(NSSearchPathDirectory.CachesDirectory, NSSearchPathDomain.User)[0];
            var path = Path.Combine(dirPath, fileName);

            object pass = new object();
            lock (pass)
            {
                if (!File.Exists(path))
                {
                    File.Create(path);
                }
            }
            return path;
        }
    }
}
