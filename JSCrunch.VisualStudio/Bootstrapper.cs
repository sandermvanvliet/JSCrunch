using System;
using Microsoft.Practices.Unity;

namespace JSCrunch.VisualStudio
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialize(IServiceProvider visualStudioServiceProvider)
        {
            var container = new UnityContainer();

            container.RegisterInstance(typeof(IServiceProvider), visualStudioServiceProvider);

            return container;
        }
    }
}