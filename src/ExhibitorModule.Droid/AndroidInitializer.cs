using ExhibitorModule.Common.Abstractions;
using ExhibitorModule.Services.Abstractions;
using Prism;
using Prism.Ioc;

namespace ExhibitorModule.Droid
{
    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IFileService, FileService>();
        }
    }
}
