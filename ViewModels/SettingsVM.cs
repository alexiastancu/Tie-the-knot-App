using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Models;
using Wedding_Planning_App.Services.Interfaces;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using Wedding_Planning_App.Data.Enums;

namespace Wedding_Planning_App.ViewModels
{
    public partial class SettingsVM : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IFiancesService _fianceService;
        private readonly IGuestService _guestService;

        public SettingsVM(IUserService userService, IFiancesService fianceService, IGuestService guestService)
        {
            _userService = userService;
            _fianceService = fianceService;
            _guestService = guestService;

            LoadUserSettings();
        }

        [ObservableProperty]
        private User user;

        [ObservableProperty]
        private Models.Fiances fiance;

        [ObservableProperty]
        private Models.Guest guest;

        [ObservableProperty]
        private bool isFiance;

        [ObservableProperty]
        private bool isGuest;

        private async void LoadUserSettings()
        {
            try
            {
                bool conversionSucceded = int.TryParse(await SecureStorage.GetAsync("userId"), out int userId);
                if (!conversionSucceded)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "There was a problem retrieving the user, try again later", "OK");
                    Debug.WriteLine("Error retrieving userId from SecureStorage or converting it to int");
                    return;
                }

                User = await _userService.GetUserById(userId);

                if (User.Role == UserRoles.Fiancés)
                {
                    IsFiance = true;
                    IsGuest = false;
                    Fiance = await _fianceService.GetFiancesByUserIdAsync(userId);
                }
                else if (User.Role == UserRoles.Guest)
                {
                    IsFiance = false;
                    IsGuest = true;
                    Guest = await _guestService.GetGuestByUserIdAsync(userId);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading user settings: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task Save()
        {
            try
            {
                await _userService.UpdateAsync(User);
                if (IsFiance)
                {
                    await _fianceService.UpdateFiancesAsync(Fiance);
                }
                else if (IsGuest)
                {
                    await _guestService.UpdateGuestAsync(Guest);
                }

                await Shell.Current.DisplayAlert("Success", "Settings saved successfully", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving settings: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "An error occurred while saving settings", "OK");
            }
        }
    }
}
