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
        ObservableCollection<ISimCard> simCards = new ObservableCollection<ISimCard>();
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

            if (simCards.First().Type == SimCardType.Standard)
            {
                selectedSimCard = (StandardSimCard)simCards.First();
            } else
            {
                selectedSimCard = (EnhancedSimCard)simCards.First();
            }

            checkAppBarButtons();
            Bindings.Update();
        }

        private void SimCardList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var simCard = (ISimCard)e.ClickedItem;

            if (simCard.Type == SimCardType.Standard)
            {
                selectedSimCard = simCard as StandardSimCard;
            } else
            {
                selectedSimCard = simCard as EnhancedSimCard;
            }

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

            int newId = simCards.Count + 1;

            if (result == ContentDialogResult.Primary)
            {
                simCards.Add(new StandardSimCard()
                {
                    id = newId,
                    MSISDN = 308285112,
                    Subscriber = string.Format("Subscriber {0}", newId),
                    PinCode = 1111,
                    Created = DateTime.Now,
                });
            } else 
            {
                simCards.Add(new EnhancedSimCard()
                {
                    id = newId,
                    MSISDN = 308285112,
                    Subscriber = string.Format("Subscriber {0}", newId),
                    PinCode = 2222,
                    Created = DateTime.Now,
                });
            }
            

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
            ReActivationButton.Visibility = Visibility.Collapsed;


            if (selectedSimCard.Status == SimCardStatus.Active)
            {
                DisableButton.Visibility = Visibility.Visible;
                GetBallanceButton.Visibility = Visibility.Visible;
            }

            if (selectedSimCard.Status == SimCardStatus.Inactive)
            {
                ActivationButton.Visibility= Visibility.Visible;
            }

            if (selectedSimCard.Status == SimCardStatus.Disabled && selectedSimCard.Type == SimCardType.Enhanced)
            {
                ReActivationButton.Visibility = Visibility.Visible;
            }
        }

        private async void ActivationButton_Click(object sender, RoutedEventArgs e)
        {
            TextBox input = new TextBox()
            {
                Height = (double)App.Current.Resources["TextControlThemeMinHeight"],
                PlaceholderText = "PIN kód"
            };
            ContentDialog dialog = new ContentDialog()
            {
                Title = "Adja meg a kártya PIN kódját",
                MaxWidth = this.ActualWidth,
                PrimaryButtonText = "OK",
                SecondaryButtonText = "Mégse",
                Content = input
            };
            ContentDialogResult result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                input = (TextBox)dialog.Content;
                bool activation = selectedSimCard.Activate(int.Parse(input.Text));
                checkAppBarButtons();
                Bindings.Update();

                if (!activation)
                {
                    await new Windows.UI.Popups.MessageDialog("Helytelen PIN kódot adott meg!").ShowAsync();
                }
            }
            
        }

        private async void ReActivationButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Reactivation button clicked");
            Debug.WriteLine(selectedSimCard);

            TextBox input = new TextBox()
            {
                Height = (double)App.Current.Resources["TextControlThemeMinHeight"],
                PlaceholderText = "PUK kód"
            };
            ContentDialog dialog = new ContentDialog()
            {
                Title = "Adja meg a kártya PUK kódját",
                MaxWidth = this.ActualWidth,
                PrimaryButtonText = "OK",
                SecondaryButtonText = "Mégse",
                Content = input
            };
            ContentDialogResult result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                input = (TextBox)dialog.Content;
                bool reActivation = selectedSimCard.ReActivate(int.Parse(input.Text));
                checkAppBarButtons();
                Bindings.Update();

                if (!reActivation)
                {
                    await new Windows.UI.Popups.MessageDialog("Helytelen PUK kódot adott meg!").ShowAsync();
                }
            }

        }

        private void DisableButton_Click(object sender, RoutedEventArgs e)
        {
            selectedSimCard.Status = SimCardStatus.Disabled;
            checkAppBarButtons();
            Bindings.Update();
        }
    }
}
