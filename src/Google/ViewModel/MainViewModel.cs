using System;
using CGeers.Web;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;

namespace Google.Phone.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IUrlShortener _urlShortener;
        private string _url;
        private string _error;
        private bool _urlFocused;
        private bool _isLoading;

        public MainViewModel(IUrlShortener urlShortener)
        {
            _urlShortener = urlShortener;

            ShortenCommand = new RelayCommand(() =>
            {
                try
                {
                    IsLoading = true;
                    Shorten();
                }
                catch(Exception)
                {
                    IsLoading = false;
                    throw;
                }
            });
        }

        public string Url
        {
            get
            {
                return _url;
            }

            set
            {
                if (_url != value)
                {
                    _url = value;
                    RaisePropertyChanged("Url");
                }
            }
        }

        public string Error
        {
            get
            {
                return _error;
            }

            set
            {
                if (_error != value)
                {
                    _error = value;
                    RaisePropertyChanged("Error");
                }
            }
        }

        public bool UrlFocused
        {
            get
            {
                return _urlFocused;
            }

            set
            {
                if (_urlFocused != value)
                {
                    _urlFocused = value;
                    RaisePropertyChanged("UrlFocused");
                }
            }
        }

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }

            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    RaisePropertyChanged("IsLoading");
                }
            }
        }

        public RelayCommand ShortenCommand { get; private set; }

        public void Shorten()
        {
            Error = string.Empty;
            UrlFocused = false;

            _urlShortener.Shorten(Url, UrlShortenCallback);
        }

        private void UrlShortenCallback(LongUrlResponse response)
        {
            try
            {
                Action updateUi;

                if (response.StatusCode == 200)
                {
                    updateUi = () => { Url = response.ShortUrl; };
                }
                else
                {
                    updateUi = () => { Error = response.Error; };
                }

                DispatcherHelper.CheckBeginInvokeOnUI(updateUi);
            }
            finally
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() => { IsLoading = false; UrlFocused = true; });
            }
        }
    }
}