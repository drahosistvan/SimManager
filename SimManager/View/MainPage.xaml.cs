using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using SimManager.Model;
using SimManager.View;
using SimManager.Data;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SimManager
{
    public sealed partial class MainPage : Page
    {
        ObservableCollection<SimCard> simCards = new ObservableCollection<SimCard>();
        SimCard selectedSimCard;
        string visualState = "";

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            simCards = SimCardsDataSource.GetAllItems();

            SimCardList.ItemsSource = simCards;
            selectedSimCard = simCards.First();
            checkAppBarButtons();
            Bindings.Update();
        }

        private void SimCardList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var simCard = (SimCard)e.ClickedItem;
            selectedSimCard = simCard;
            Debug.WriteLine(simCard.GetInfo());
            checkAppBarButtons();

            Bindings.Update();

            if (visualState == "NarrowState")
            {
                this.Frame.Navigate(typeof(DetailPage), simCard);
            }
        }

        private void AdaptiveStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            visualState = e.NewState.Name;
            Debug.WriteLine("State changed: " + visualState);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog newSimCard = new ContentDialog
            {
                Title = "Új SIM kártya hozzáadása",
                Content = "Kérjük válassza ki a kártya típusát",
                PrimaryButtonText = "Egyszerű SIM",
                SecondaryButtonText = "Továbbfejlesztett SIM"
            };

            ContentDialogResult result = await newSimCard.ShowAsync();

            simCards.Add(new SimCard()
            {
                id = 10,
                MSISDN = 308285112,
                Subscriber = "Drahos István",
                PinCode = 1234,
                Created = DateTime.Now,
            });

            Debug.WriteLine(result);
        }

        private async void SimBalance_Click(object sender, RoutedEventArgs e)
        {

            ContentDialog balanceDialog = new ContentDialog
            {
                Title = "Egyenleg",
                Content = string.Format("Egyenleg mutatása: {0}", selectedSimCard.GetBalance()),
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await balanceDialog.ShowAsync();
        }

        private void checkAppBarButtons()
        {
            GetBallanceButton.Visibility = Visibility.Collapsed;
            ActivationButton.Visibility = Visibility.Collapsed;
            DisableButton.Visibility = Visibility.Collapsed;


            if (selectedSimCard.Status == SimCardStatus.Active)
            {
                DisableButton.Visibility = Visibility.Visible;
                GetBallanceButton.Visibility = Visibility.Visible;
            }

            if (selectedSimCard.Status == SimCardStatus.Inactive || selectedSimCard.Status == SimCardStatus.Disabled)
            {
                ActivationButton.Visibility= Visibility.Visible;
            }
        }

        private void ActivationButton_Click(object sender, RoutedEventArgs e)
        {
            selectedSimCard.Status = SimCardStatus.Active;
            checkAppBarButtons();
            Bindings.Update();
        }

        private void DisableButton_Click(object sender, RoutedEventArgs e)
        {
            selectedSimCard.Status = SimCardStatus.Disabled;
            checkAppBarButtons();
            Bindings.Update();
        }
    }
}
