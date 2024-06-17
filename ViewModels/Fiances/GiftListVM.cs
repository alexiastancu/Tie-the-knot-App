using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Data.Enums;
using Wedding_Planning_App.Models;
using Wedding_Planning_App.Services.Interfaces;

namespace Wedding_Planning_App.ViewModels.Fiances
{
    public partial class GiftListVM : ObservableRecipient
    {
        private readonly IGiftService _giftService;
        private readonly IUserService _userService;

        public GiftListVM()
        {
            
        }
        public GiftListVM(IGiftService giftService, IUserService userService)
        {
            _giftService = giftService;
            _userService = userService;

            LoadUserRole();
        }

        [ObservableProperty]
        private int weddingId;

        [ObservableProperty]
        private ObservableCollection<Gift> gifts;

        [ObservableProperty]
        private Gift selectedGift;

        [ObservableProperty]
        private string name;


        [ObservableProperty]
        private decimal price;

        [ObservableProperty]
        private string storeLink;

        [ObservableProperty]
        private bool isFiance;

        [RelayCommand]
        public async Task LoadGifts()
        {
            if(IsFiance)
            {
                bool conversionSucceded = int.TryParse(await SecureStorage.GetAsync("weddingId"), out weddingId);
                if (!conversionSucceded)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "There was a problem retrieving the wedding, try again later", "OK");
                    Debug.WriteLine("Error retrieving weddingId from SecureStorage or converting it to int");
                    return;
                }
            }
            var gifts = await _giftService.GetGiftsByWeddingIdAsync(WeddingId);
            Gifts = new ObservableCollection<Gift>(gifts);
        }

        [RelayCommand]
        private async Task AddGift()
        {
            var gift = new Gift
            {
                WeddingId = weddingId,
                Name = Name,
                Price = Price,
                StoreLink = StoreLink,
                IsPurchased = false
            };
            await _giftService.AddGiftAsync(gift);
            await LoadGifts();
            Name = string.Empty;
            Price = 0;
            StoreLink = string.Empty;
        }

        [RelayCommand]
        private async Task UpdateGift()
        {
            if (SelectedGift != null)
            {
                SelectedGift.Name = Name;
                SelectedGift.Price = Price;
                SelectedGift.StoreLink = StoreLink;
                await _giftService.UpdateGiftAsync(SelectedGift);
                await LoadGifts();
            }
        }

        [RelayCommand]
        private async Task DeleteGift()
        {
            if (SelectedGift != null)
            {
                await _giftService.DeleteGiftAsync(SelectedGift.Id);
                await LoadGifts();
            }
        }

        [RelayCommand]
        private async Task PurchaseGift(Gift gift)
        {
            if (gift != null && !gift.IsPurchased)
            {
                await Application.Current.MainPage.DisplayAlert("Gift purchased", "Are you sure you want to purchase this gift?", "YES");
                gift.IsPurchased = true;
                await _giftService.UpdateGiftAsync(gift);
                await LoadGifts();
            }
        }

        public async void LoadUserRole()
        {
            bool conversionSucceded = int.TryParse(await SecureStorage.GetAsync("userId"), out int userId);
            if (!conversionSucceded)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "There was a problem retrieving the user, try again later", "OK");
                Debug.WriteLine("Error retrieving weddingId from SecureStorage or converting it to int");
                return;
            }
            var role = await _userService.GetUserRoleAsync(userId);
            IsFiance = role == UserRoles.Fiancés;
        }

        partial void OnSelectedGiftChanged(Gift value)
        {
            if (value != null)
            {
                Name = value.Name;
                Price = value.Price;
                StoreLink = value.StoreLink;
            }
        }

        [RelayCommand]
        private async Task OnGiftLongPressed(Gift gift)
        {
            if (gift == null) return;

            bool confirm = await Application.Current.MainPage.DisplayAlert("Open Link", "Do you want to open the gift link?", "Yes", "No");
            if (confirm)
            {
                try
                {
                    Uri uri = new Uri(gift.StoreLink);
                    await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                }
                catch (Exception ex)
                {
                    // Handle exception
                    await Application.Current.MainPage.DisplayAlert("Error", "Unable to open link", "OK");
                }
            }
        }
    }
}
