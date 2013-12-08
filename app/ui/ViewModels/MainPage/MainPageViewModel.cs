﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using VLC_WINRT.Common;
using VLC_WINRT.Utility.Commands;

namespace VLC_WINRT.ViewModels.MainPage
{
    public class MainPageViewModel : NavigateableViewModel
    {
        private ObservableCollection<LibraryViewModel> _dlnaVMs =
            new ObservableCollection<LibraryViewModel>();

        private bool _isAppBarOpen;
        private bool _isNetworkAppBarShown;
        private LastViewedViewModel _lastViewedVM;
        private LibraryViewModel _musicVM;
        private string _networkMRL = string.Empty;
        private PickVideoCommand _pickVideoCommand;
        private PlayNetworkMRLCommand _playNetworkMRL;

        private ObservableCollection<LibraryViewModel> _removableStorageVMs =
            new ObservableCollection<LibraryViewModel>();

        private ActionCommand _showAppBarCommand;

        private ActionCommand _toggleNetworkAppBarCommand;
        private LibraryViewModel _videoVM;

        public MainPageViewModel()
        {
            VideoVM = new LibraryViewModel(KnownVLCLocation.VideosLibrary);
            MusicVM = new LibraryViewModel(KnownVLCLocation.MusicLibrary);

            Task<IReadOnlyList<StorageFolder>> dlnaFolders = KnownVLCLocation.MediaServers.GetFoldersAsync().AsTask();
            dlnaFolders.ContinueWith(t =>
            {
                IReadOnlyList<StorageFolder> folders = t.Result;
                foreach (StorageFolder storageFolder in folders)
                {
                    StorageFolder newFolder = storageFolder;
                    DispatchHelper.Invoke(() => DLNAVMs.Add(new LibraryViewModel(newFolder)));
                }
            });

            LastViewedVM = new LastViewedViewModel();
            PickVideo = new PickVideoCommand();
            PlayNetworkMRL = new PlayNetworkMRLCommand();

            _toggleNetworkAppBarCommand =
                new ActionCommand(() => { IsNetworkAppBarShown = !IsNetworkAppBarShown; });

            _showAppBarCommand = new ActionCommand(() => { IsAppBarOpen = true; });
        }

        public LibraryViewModel VideoVM
        {
            get { return _videoVM; }
            set { SetProperty(ref _videoVM, value); }
        }

        public LibraryViewModel MusicVM
        {
            get { return _musicVM; }
            set { SetProperty(ref _musicVM, value); }
        }

        public ObservableCollection<LibraryViewModel> DLNAVMs
        {
            get { return _dlnaVMs; }
            set { SetProperty(ref _dlnaVMs, value); }
        }

        public LastViewedViewModel LastViewedVM
        {
            get { return _lastViewedVM; }
            set { SetProperty(ref _lastViewedVM, value); }
        }

        public bool IsNetworkAppBarShown
        {
            get { return _isNetworkAppBarShown; }
            set { SetProperty(ref _isNetworkAppBarShown, value); }
        }

        public PickVideoCommand PickVideo
        {
            get { return _pickVideoCommand; }
            set { SetProperty(ref _pickVideoCommand, value); }
        }

        public ActionCommand ShowAppBarCommand
        {
            get { return _showAppBarCommand; }
            set { SetProperty(ref _showAppBarCommand, value); }
        }

        public ActionCommand ToggleNetworkAppBarCommand
        {
            get { return _toggleNetworkAppBarCommand; }
            set { SetProperty(ref _toggleNetworkAppBarCommand, value); }
        }

        public ObservableCollection<LibraryViewModel> RemovableStorageVMs
        {
            get { return _removableStorageVMs; }
            set { SetProperty(ref _removableStorageVMs, value); }
        }

        public bool IsAppBarOpen
        {
            get { return _isAppBarOpen; }
            set
            {
                SetProperty(ref _isAppBarOpen, value);
                if (value == false)
                {
                    // hide open network portion of appbar whenever app bar is dissmissed.
                    IsNetworkAppBarShown = false;
                }
            }
        }

        public PlayNetworkMRLCommand PlayNetworkMRL
        {
            get { return _playNetworkMRL; }
            set { SetProperty(ref _playNetworkMRL, value); }
        }

        public string NetworkMRL
        {
            get { return _networkMRL; }
            set { SetProperty(ref _networkMRL, value); }
        }
    }
}