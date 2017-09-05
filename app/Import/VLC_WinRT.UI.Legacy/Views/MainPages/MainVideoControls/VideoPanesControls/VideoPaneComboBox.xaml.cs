﻿using Windows.UI.Xaml.Controls;
using VLC.ViewModels;

#if WINDOWS_PHONE_APP
using Windows.Phone.UI.Input;
#endif

namespace VLC_WinRT.Views.MainPages.MainVideoControls.VideoPanesControls
{
    public sealed partial class VideoPaneComboBox : UserControl
    {
        public VideoPaneComboBox()
        {
            this.InitializeComponent();
#if WINDOWS_PHONE_APP
            this.Loaded += VideoPaneComboBox_Loaded;
#endif
        }

#if WINDOWS_PHONE_APP
        void VideoPaneComboBox_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            this.Unloaded += VideoPaneComboBox_Unloaded;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (VideoViewComboBox.IsDropDownOpen)
            {
                VideoViewComboBox.IsDropDownOpen = false;
                e.Handled = true;
            }
        }

        void VideoPaneComboBox_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }
#endif

        private void ComboBox_OnDropDownOpened(object sender, object e)
        {
            Locator.NavigationService.PreventAppExit = true;
        }

        private void ComboBox_OnDropDownClosed(object sender, object e)
        {
            Locator.NavigationService.PreventAppExit = false;
        }
    }
}
