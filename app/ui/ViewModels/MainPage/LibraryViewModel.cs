﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using VLC_WINRT.Common;
using VLC_WINRT.Utility.Services;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.System.Threading;

namespace VLC_WINRT.ViewModels.MainPage
{
    public class LibraryViewModel : BindableBase
    {
        private StorageFolder _location;
        private ObservableCollection<MediaViewModel> _media; 
        private string _title;

        public LibraryViewModel(StorageFolder location)
        {
            Media = new ObservableCollection<MediaViewModel>();
            Location = location;
            Title = location.DisplayName;

            //Get off UI thread
            ThreadPool.RunAsync(GetMedia);
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public StorageFolder Location
        {
            get { return _location; }
            set { SetProperty(ref _location, value); }
        }

        public ObservableCollection<MediaViewModel> Media
        {
            get { return _media; }
            set { SetProperty(ref _media, value); }
        }

        protected async void GetMedia(IAsyncAction operation)
        {
            IEnumerable<StorageFile> files =
                await MediaScanner.GetMediaFromFolder(_location, 6, CommonFileQuery.OrderByDate);
            foreach (StorageFile storageFile in files)
            {
                var mediaVM = new MediaViewModel(storageFile);

                // Get back to UI thread
                DispatchHelper.Invoke(() => Media.Add(mediaVM));
            }
        }
    }
}