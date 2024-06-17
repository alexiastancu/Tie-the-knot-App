using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wedding_Planning_App.Models;
using Wedding_Planning_App.Services.Interfaces;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace Wedding_Planning_App.ViewModels.Fiances
{
    public partial class SeatGuestPopupVM : ObservableObject
    {
        private readonly IGuestService _guestService;
        private readonly IGuestSeatService _guestSeatService;
        private readonly Popup _popup;

        [ObservableProperty]
        private string searchQuery;

        [ObservableProperty]
        private ObservableCollection<Models.Guest> filteredGuests;

        [ObservableProperty]
        private Models.Guest selectedGuest;

        [ObservableProperty]
        private GuestSeat guestSeat;


        public SeatGuestPopupVM(GuestSeat _guestSeat, Popup popup)
        {
            _guestService = MauiProgram.CreateMauiApp().Services.GetRequiredService<IGuestService>();
            _guestSeatService = MauiProgram.CreateMauiApp().Services.GetRequiredService<IGuestSeatService>();
            _popup = popup;
            guestSeat = _guestSeat;
            FilteredGuests = new ObservableCollection<Models.Guest>();

            // Load all guests initially
            LoadGuests();
        }

        private async void LoadGuests()
        {
            // Load guests invited to the wedding
            bool conversionSucceded = int.TryParse(await SecureStorage.GetAsync("weddingId"), out int weddingId);
            if (!conversionSucceded)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "There was a problem retrieving the wedding, try again later", "OK");
                Debug.WriteLine("Error retrieving weddingId from SecureStorage or converting it to int");
                return;
            }
            var unassignedGuests = await _guestService.GetUnassignedGuestsByWeddingIdAsync(weddingId);
            FilteredGuests = new ObservableCollection<Models.Guest>(unassignedGuests);
        }

        [RelayCommand]
        private void OnSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                LoadGuests();
            }
            else
            {
                var filtered = FilteredGuests.Where(g => g.User.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                                                        g.User.Surname.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                                                        g.User.Email.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
                FilteredGuests = new ObservableCollection<Models.Guest>(filtered);
            }
        }

        [RelayCommand]
        private async void OnConfirm()
        {
            if (SelectedGuest != null)
            {
                guestSeat.GuestId = SelectedGuest.Id;
                guestSeat.IsOccupied = true;
                await _guestSeatService.UpdateGuestSeatAsync(guestSeat);
                _popup.Close(SelectedGuest);
            }
        }

        [RelayCommand]
        private void OnCancel()
        {
            _popup.Close(null);
        }

    }
}
