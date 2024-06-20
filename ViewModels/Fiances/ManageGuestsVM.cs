using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Models;
using Wedding_Planning_App.Services.Interfaces;

namespace Wedding_Planning_App.ViewModels.Fiances
{
    public partial class ManageGuestsVM : ObservableObject
    {
        private readonly IGuestService _guestService;
        private readonly IUserService _userService;
        private readonly IWeddingGuestService _weddingGuestService;

        public ManageGuestsVM()
        {
            
        }
        public ManageGuestsVM(IGuestService guestService, IWeddingGuestService weddingGuestService)
        {
            _guestService = guestService;
            _weddingGuestService = weddingGuestService;
            LoadGuests();
        }

        [ObservableProperty]
        private ObservableCollection<WeddingGuestIntermediate> guests;

        [ObservableProperty]
        private Models.Guest selectedGuest;

        private int weddingId;

        public async void LoadGuests()
        {
            try
            {
                bool conversionSucceded = int.TryParse(await SecureStorage.GetAsync("weddingId"), out weddingId);
                if (!conversionSucceded)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "There was a problem retrieving the wedding, try again later", "OK");
                    Debug.WriteLine("Error retrieving weddingId from SecureStorage or converting it to int");
                }
                var guestList = await _weddingGuestService.GetGuestsByWeddingIdAsync(weddingId);
                Guests = new ObservableCollection<WeddingGuestIntermediate>(guestList);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading guests: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task DeleteGuest(WeddingGuestIntermediate weddingGuest)
        {
            bool confirm = await Shell.Current.DisplayAlert("Confirm", "Are you sure you want to delete this guest?", "Yes", "No");
            if (confirm)
            {
                try
                {
                    if (weddingGuest == null)
                    {
                        return;
                    }
                    await _weddingGuestService.RemoveGuestFromWeddingAsync(weddingId, weddingGuest.GuestId);
                    Guests.Remove(weddingGuest);
                    var tempGuests = Guests;
                    Guests = null;
                    Guests = tempGuests;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error deleting guest: {ex.Message}");
                    await Shell.Current.DisplayAlert("Error", "An error occurred while deleting the guest", "OK");
                }
            }
        }
    }
}
