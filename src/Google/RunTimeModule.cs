using CGeers.Web;
using Google.Phone.UI.ViewModel;
using Ninject;

namespace Google.Phone.UI
{
    public class RunTimeModule : IIocContainer
    {
        private static readonly IKernel Kernel = new StandardKernel();

        static RunTimeModule()
        {
            Kernel.Bind<INavigationService>().To<NavigationService>().InSingletonScope();
            Kernel.Bind<IUrlShortener>().To<CGeers.Google.Google>();
        }

        public T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }
}
