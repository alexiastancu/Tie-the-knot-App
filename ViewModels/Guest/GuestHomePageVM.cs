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
using Wedding_Planning_App.Views.Guest;

namespace Wedding_Planning_App.ViewModels.Guest
{
    [QueryProperty(nameof(User), nameof(User))]
    public partial class GuestHomePageVM : ObservableRecipient
    {
        private readonly IGuestService _guestService;
        private readonly IWeddingService _weddingService;
        private readonly IWeddingGuestService _weddingGuestIntermediateService;

        #region Properties
        [ObservableProperty]
        private User user;
        [ObservableProperty]
        private Models.Guest guest;
        #endregion


        [ObservableProperty]
        private ObservableCollection<Wedding> guestWeddings;

        public GuestHomePageVM(IWeddingService weddingService, IWeddingGuestService weddingGuestIntermediateService, IGuestService guestService)
        {
            _weddingService = weddingService;
            _weddingGuestIntermediateService = weddingGuestIntermediateService;
            _guestService = guestService;
            //LoadGuestWeddings();
        }

        public GuestHomePageVM()
        {
            
        }

        public async void LoadGuestWeddings()
        {
            bool conversionSucceded = int.TryParse(await SecureStorage.GetAsync("userId"), out int userId);
            if (!conversionSucceded)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "There was a problem retrieving the user, try again later", "OK");
                Debug.WriteLine("Error retrieving userId from SecureStorage or converting it to int");
                return;
            }
            Guest = await _guestService.GetGuestByUserIdAsync(userId);
            var weddingIds = await _weddingGuestIntermediateService.GetWeddingsByGuestIdAsync(Guest.Id);
            var weddings = await _weddingService.GetWeddingsByIdsAsync(weddingIds);
            GuestWeddings = new ObservableCollection<Wedding>(weddings);
        }

        [RelayCommand]
        private async Task WeddingSelected(Wedding wedding)
        {
            await Shell.Current.GoToAsync($"{nameof(WeddingDetails)}?{"WeddingId"}={wedding.Id}");
            
        }
    }
}
