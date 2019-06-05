using ExhibitorModule.Common.Abstractions;
using ExhibitorModule.Services.Abstractions;
using Prism;
using Prism.Ioc;

namespace ExhibitorModule.iOS
{
    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IFileService, FileService>();
        }
    }
}
