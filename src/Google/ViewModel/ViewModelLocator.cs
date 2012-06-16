using System;
using CGeers.Web;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace Google.Phone.UI.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    { 
        private static MainViewModel _main;
        private static AboutViewModel _about;

        public static readonly Uri MainPageUri = new Uri("/View/MainPage.xaml", UriKind.Relative);

        public static readonly Uri AboutPageUri = new Uri("/View/About.xaml", UriKind.Relative);

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            IIocContainer container;
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time services and viewmodels
                container = new DesignTimeModule();
            }
            else
            {
                // Create run time services and view models
                container = new RunTimeModule();
            }

            var navigationService = container.Get<INavigationService>();

            Messenger.Default.Register<MoveToViewMessage>(this, message =>
            {
                switch (message.Page)
                {
                    case Page.MainPage:
                        navigationService.NavigateTo(MainPageUri);
                        break;

                    case Page.About:
                        navigationService.NavigateTo(AboutPageUri);
                        break;
                }
            });

            _main = new MainViewModel(container.Get<IUrlShortener>());
            _about = new AboutViewModel();
        }

        /// <summary>
        /// Gets the Main property which defines the main viewmodel.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return _main;
            }
        }

        /// <summary>
        /// Gets the About property which defines the about viewmodel.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AboutViewModel About
        {
            get
            {
                return _about;
            }
        }
    }
}