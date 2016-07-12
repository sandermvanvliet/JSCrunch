using JSCrunch.Core;
using Microsoft.Practices.Unity;

namespace JSCrunch
{
    public class Bootstrapper
    {
        public static IUnityContainer Boot()
        {
            var container = new UnityContainer();

            container.RegisterType<IOutput, ConsoleOutput>();
            container.RegisterInstance(new EventQueue());

            return container;
        }
    }
}