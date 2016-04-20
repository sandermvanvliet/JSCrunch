using Microsoft.Practices.Unity;

namespace JSCrunch
{
    public class Bootstrapper
    {
        public static IUnityContainer Boot()
        {
            var container = new UnityContainer();

            container.RegisterType<IOutput, ConsoleOutput>();

            return container;
        }
    }
}