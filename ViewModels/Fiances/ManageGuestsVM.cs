using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public ManageGuestsVM(IGuestService guestService, IUserService userService, IWeddingGuestService weddingGuestService)
        {
            _guestService = guestService;
            _userService = userService;
            _weddingGuestService = weddingGuestService;
            LoadGuests();
        }

        [ObservableProperty]
        private ObservableCollection<Models.Guest> guests;

        [ObservableProperty]
        private Models.Guest selectedGuest;

        private int weddingId;

        private async void LoadGuests()
        {
            try
            {
                bool conversionSucceded = int.TryParse(await SecureStorage.GetAsync("weddingId"), out weddingId);
                if (!conversionSucceded)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "There was a problem retrieving the wedding, try again later", "OK");
                    Debug.WriteLine("Error retrieving weddingId from SecureStorage or converting it to int");
                }
                var guestList = await _guestService.GetGuestsByWeddingIdAsync(weddingId);
                Guests = new ObservableCollection<Models.Guest>(guestList);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading guests: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task DeleteGuest(Models.Guest guest)
        {
            bool confirm = await Shell.Current.DisplayAlert("Confirm", "Are you sure you want to delete this guest?", "Yes", "No");
            if (confirm)
            {
                try
                {
                    await _weddingGuestService.RemoveGuestFromWeddingAsync(guest.Id, weddingId);
                    //LoadGuests();
                    Guests.Remove(guest);
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
