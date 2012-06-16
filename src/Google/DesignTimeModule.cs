using CGeers.Web;
using Google.Phone.UI.ViewModel;
using Ninject;

namespace Google.Phone.UI
{
    public class DesignTimeModule : IIocContainer
    {
        private static readonly IKernel Kernel = new StandardKernel();

        static DesignTimeModule()
        {
            Kernel.Bind<INavigationService>().To<NavigationService>();
            Kernel.Bind<IUrlShortener>().To<CGeers.Google.Google>();
        }

        public T Get<T>()
        {
            return Kernel.Get<T>();
        }
    }
}
