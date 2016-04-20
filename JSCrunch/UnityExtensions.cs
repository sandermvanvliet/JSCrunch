using Microsoft.Practices.Unity;

namespace JSCrunch
{
    public static class UnityExtensions
    {
        public static TType Resolve<TType>(this IUnityContainer unityContainer)
        {
            return (TType)unityContainer.Resolve(typeof(TType));
        }
    }
}