using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Models;
using Wedding_Planning_App.Services.Interfaces;

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
            Guest = await _guestService.GetGuestByUserIdAsync(User.Id);
            var weddingIds = await _weddingGuestIntermediateService.GetWeddingsByGuestIdAsync(Guest.Id);
            var weddings = await _weddingService.GetWeddingsByIdsAsync(weddingIds);
            GuestWeddings = new ObservableCollection<Wedding>(weddings);
        }

        [RelayCommand]
        private async void WeddingSelected(Wedding wedding)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "you have selected a wedding", "OK");
            //for testing purposes
            //var list = await _weddingGuestIntermediateService.GetGuestListAsync();
        }
    }
}
