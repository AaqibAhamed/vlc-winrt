﻿/**********************************************************************
 * VLC for WinRT
 **********************************************************************
 * Copyright © 2013-2014 VideoLAN and Authors
 *
 * Licensed under GPLv2+ and MPLv2
 * Refer to COPYING file of the official project for license
 **********************************************************************/

#if WINDOWS_PHONE_APP

#endif
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using SQLite;
using VLC_WINRT.Common;
using VLC_WINRT_APP.Commands;
using VLC_WINRT_APP.Commands.Video;
using VLC_WINRT_APP.Common;
using VLC_WINRT_APP.DataRepository;
using VLC_WINRT_APP.Helpers;
using VLC_WINRT_APP.Helpers.VideoLibrary;
using VLC_WINRT_APP.Model;
using VLC_WINRT_APP.Model.Video;
using WinRTXamlToolkit.IO.Extensions;
using Panel = VLC_WINRT_APP.Model.Panel;
using VLC_WINRT_APP.Model.Search;

namespace VLC_WINRT_APP.ViewModels.VideoVM
{
    public class VideoLibraryVM : BindableBase
    {
        public VideoRepository VideoRepository = new VideoRepository();
        #region private fields
        private ObservableCollection<VideoItem> _searchResults = new ObservableCollection<VideoItem>();
#if WINDOWS_APP
        private ObservableCollection<Panel> _panels = new ObservableCollection<Panel>();
#endif
        private ObservableCollection<VideoItem> _videos;
        private ObservableCollection<VideoItem> _viewedVideos;
        private ObservableCollection<VideoItem> _cameraRoll;
        private ObservableCollection<TvShow> _shows = new ObservableCollection<TvShow>();
        #endregion

        #region private props
        private LoadingState _loadingState;
        private PlayVideoCommand _openVideo;
        private PickVideoCommand _pickCommand = new PickVideoCommand();
        private PlayNetworkMRLCommand _playNetworkMRL = new PlayNetworkMRLCommand();
        private bool _hasNoMedia = true;
        private TvShow _currentShow;
        private CloseFlyoutAndPlayVideoCommand _closeFlyoutAndPlayVideoCommand;

        private string _searchTag;
        #endregion

        #region public fields
        public ObservableCollection<VideoItem> SearchResults
        {
            get { return _searchResults; }
            set { SetProperty(ref _searchResults, value); }
        }
#if WINDOWS_APP
        public ObservableCollection<Panel> Panels
        {
            get { return _panels; }
            set
            {
                SetProperty(ref _panels, value);
            }
        }
#endif

        public ObservableCollection<VideoItem> Videos
        {
            get { return _videos; }
            set { SetProperty(ref _videos, value); }
        }

        public ObservableCollection<VideoItem> ViewedVideos
        {
            get { return _viewedVideos; }
            set
            {
                SetProperty(ref _viewedVideos, value);
            }
        }

        public ObservableCollection<TvShow> Shows
        {
            get { return _shows; }
            set { SetProperty(ref _shows, value); }
        }

        public ObservableCollection<VideoItem> CameraRoll
        {
            get { return _cameraRoll; }
            set { SetProperty(ref _cameraRoll, value); }
        }
        #endregion

        #region public props

        public TvShow CurrentShow
        {
            get { return _currentShow; }
            set { SetProperty(ref _currentShow, value); }
        }

        public LoadingState LoadingState { get { return _loadingState; } set { SetProperty(ref _loadingState, value); } }
        public bool HasNoMedia
        {
            get { return _hasNoMedia; }
            set { SetProperty(ref _hasNoMedia, value); }
        }

        [Ignore]
        public PlayVideoCommand OpenVideo
        {
            get { return _openVideo; }
            set { SetProperty(ref _openVideo, value); }
        }

        [Ignore]
        public CloseFlyoutAndPlayVideoCommand CloseFlyoutAndPlayVideoCommand
        {
            get { return _closeFlyoutAndPlayVideoCommand ?? (_closeFlyoutAndPlayVideoCommand = new CloseFlyoutAndPlayVideoCommand()); }
        }

        public PickVideoCommand PickVideo
        {
            get { return _pickCommand; }
            set { SetProperty(ref _pickCommand, value); }
        }

        public PlayNetworkMRLCommand PlayNetworkMRL
        {
            get { return _playNetworkMRL; }
            set { SetProperty(ref _playNetworkMRL, value); }
        }

        public string SearchTag
        {
            get { return _searchTag; }
            set
            {
                if (string.IsNullOrEmpty(_searchTag) && !string.IsNullOrEmpty(value))
                    Locator.MainVM.ChangeMainPageVideoViewCommand.Execute(3);
                if (!string.IsNullOrEmpty(value))
                    SearchHelpers.SearchVideos(value, SearchResults);
                SetProperty(ref _searchTag, value);
            }
        }
        #endregion
        #region contructors
        public VideoLibraryVM()
        {
            LoadingState = LoadingState.NotLoaded;
            OpenVideo = new PlayVideoCommand();
            Videos = new ObservableCollection<VideoItem>();
            ViewedVideos = new ObservableCollection<VideoItem>();
            CameraRoll = new ObservableCollection<VideoItem>();
#if WINDOWS_APP
            var resourceLoader = new ResourceLoader();
            Panels.Add(new Panel(resourceLoader.GetString("Videos"), 0, App.Current.Resources["VideoPath"].ToString(), true));
            //Panels.Add(new Panel("favorite", 2, 0.4));
#endif
        }

        public async Task Initialize()
        {
            await App.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => LoadingState = LoadingState.Loading);
            await VideoLibraryManagement.GetViewedVideos().ConfigureAwait(false);
            await VideoLibraryManagement.GetVideos(VideoRepository).ConfigureAwait(false);
            await VideoLibraryManagement.GetVideosFromCameraRoll(VideoRepository).ConfigureAwait(false);
        }
        #endregion

        #region methods
        public void ExecuteSemanticZoom(SemanticZoom sZ, CollectionViewSource cvs)
        {
            (sZ.ZoomedOutView as ListViewBase).ItemsSource = cvs.View.CollectionGroups;
        }
        #endregion
    }
}
