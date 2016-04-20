using Microsoft.Practices.Unity;

namespace JSCrunch
{
    public class Bootstrapper
    {
        public static IUnityContainer Boot()
        {
            return new UnityContainer();
        }
    }
}