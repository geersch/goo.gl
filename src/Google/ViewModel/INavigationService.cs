using System;
using System.Windows.Navigation;

namespace Google.Phone.UI.ViewModel
{
    public interface INavigationService
    {
        event NavigatingCancelEventHandler Navigating;

        void NavigateTo(Uri pageUri);

        void GoBack();
    }
}
