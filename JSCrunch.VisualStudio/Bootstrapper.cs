using Microsoft.Practices.Unity;

namespace JSCrunch.VisualStudio
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialize()
        {
            var container = new UnityContainer();



            return container;
        }
    }
}