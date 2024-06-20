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
using Wedding_Planning_App.Data.Enums;

namespace Wedding_Planning_App.ViewModels.Fiances
{
    public partial class SeatGuestPopupVM : ObservableObject
    {
        private readonly IGuestService _guestService;
        private readonly IGuestSeatService _guestSeatService;
        private readonly IWeddingGuestService _weddingGuestService;
        private readonly Popup _popup;

        [ObservableProperty]
        private string searchQuery;

        [ObservableProperty]
        private ObservableCollection<WeddingGuestIntermediate> filteredGuests;

        [ObservableProperty]
        private WeddingGuestIntermediate selectedGuestIntermediate;

        [ObservableProperty]
        private GuestSeat guestSeat;


        public SeatGuestPopupVM(GuestSeat _guestSeat, Popup popup)
        {
            _guestService = MauiProgram.CreateMauiApp().Services.GetRequiredService<IGuestService>();
            _guestSeatService = MauiProgram.CreateMauiApp().Services.GetRequiredService<IGuestSeatService>();
            _weddingGuestService = MauiProgram.CreateMauiApp().Services.GetRequiredService<IWeddingGuestService>();
            _popup = popup;
            guestSeat = _guestSeat;
            FilteredGuests = new ObservableCollection<WeddingGuestIntermediate>();

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
            //var unassignedGuests = await _guestService.GetUnassignedGuestsByWeddingIdAsync(weddingId);
            //FilteredGuests = new ObservableCollection<Models.Guest>(unassignedGuests);

            var unassignedGuests = await _weddingGuestService.GetUnassignedGuestsByWeddingIdAsync(weddingId);
            var acceptedGuests = unassignedGuests
                .Where(wg => wg.InvitationStatus == InvitationStatus.Accepted)
                .ToList();

            FilteredGuests = new ObservableCollection<WeddingGuestIntermediate>(acceptedGuests);
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
                var filtered = FilteredGuests.Where(wg => wg.Guest.User.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                                                          wg.Guest.User.Surname.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                                                          wg.Guest.User.Email.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
                FilteredGuests = new ObservableCollection<WeddingGuestIntermediate>(filtered);
            }
        }

        [RelayCommand]
        private async void OnConfirm()
        {
            if (SelectedGuestIntermediate != null)
            {
                guestSeat.GuestId = SelectedGuestIntermediate.Guest.Id;
                guestSeat.IsOccupied = true;
                await _guestSeatService.UpdateGuestSeatAsync(guestSeat);
                _popup.Close(SelectedGuestIntermediate.Guest);
            }
        }

        [RelayCommand]
        private void OnCancel()
        {
            _popup.Close(null);
        }

    }
}
