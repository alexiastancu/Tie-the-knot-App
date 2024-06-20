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
    public partial class AddGuestVM : ObservableRecipient
    {
        private readonly IUserService _userService;
        private readonly IGuestService _guestService;
        private readonly IWeddingGuestService _weddingGuestService;

        public ObservableCollection<User> FilteredUsers { get; } = new ObservableCollection<User>();

        private int weddingId;
        private Wedding wedding;

        [ObservableProperty]
        private string searchQuery;

        [ObservableProperty]
        private User selectedUser;

        [ObservableProperty]
        private string newGuestName;

        [ObservableProperty]
        private string newGuestSurname;

        [ObservableProperty]
        private string newGuestEmail;

        [ObservableProperty]
        private string newGuestPhoneNumber;

        [ObservableProperty]
        private string newGuestDietaryRestrictions;

        public AddGuestVM(IGuestService guestService, IWeddingGuestService weddingGuestService, IUserService userService)
        {
            _guestService = guestService;
            _weddingGuestService = weddingGuestService;
            _userService = userService;
            LoadUsers();

        }

        private async void LoadUsers()
        {
            var users = await _userService.GetUserList();
            foreach (var user in users)
            {
                if (user.Role == UserRoles.Guest)
                    FilteredUsers.Add(user);
            }
            bool conversionSucceded = int.TryParse(await SecureStorage.GetAsync("weddingId"), out weddingId);
            if(!conversionSucceded)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "There was a problem retrieving the wedding, try again later", "OK");
                Debug.WriteLine("Error retrieving weddingId from SecureStorage or converting it to int");
            }
        }

        partial void OnSearchQueryChanged(string value)
        {
            FilterGuests();
        }

        private void FilterGuests()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                FilteredUsers.Clear();
                LoadUsers();
            }
            else
            {
                var filtered = FilteredUsers.Where(u => u.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                                                        u.Surname.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||  
                                                        u.Email.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
                FilteredUsers.Clear();
                foreach (var user in filtered)
                {
                    FilteredUsers.Add(user);
                }
            }
        }

        [RelayCommand]
        public async System.Threading.Tasks.Task AddGuestAsync()
        {
            if (SelectedUser != null)
            {
                var isUserAlreadyAdded = await IsSelectedUserAlreadyAddedAsync(weddingId, SelectedUser.Id);
                if (isUserAlreadyAdded)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "This user is already added to the wedding", "OK");
                    ClearFields();
                    return;
                }
                var guest = await _guestService.GetGuestByUserIdAsync(SelectedUser.Id);

                await _weddingGuestService.AddGuestToWeddingAsync(weddingId, guest.Id);

                await Application.Current.MainPage.DisplayAlert("Success", "Existing user added to wedding successfully", "OK");
            }
            else if (!string.IsNullOrWhiteSpace(NewGuestName) && !string.IsNullOrWhiteSpace(NewGuestSurname) &&
                     !string.IsNullOrWhiteSpace(NewGuestEmail) && !string.IsNullOrWhiteSpace(NewGuestPhoneNumber))
            {
                var existingUser = await _userService.FindByEmailAsync(NewGuestEmail);
                if (existingUser != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "A user with this email already exists", "OK");
                    ClearFields();
                    return;
                }

                var newUser = new User
                {
                    Name = NewGuestName,
                    Surname = NewGuestSurname,
                    Email = NewGuestEmail,
                    PhoneNumber = NewGuestPhoneNumber,
                    Role = UserRoles.Guest
                };
                await _userService.CreateAsync(newUser);

                var newGuest = new Models.Guest
                {
                    DietaryRestrictions = NewGuestDietaryRestrictions,
                    UserId = newUser.Id,
                };

                await _guestService.AddGuestAsync(newGuest);

                await _weddingGuestService.AddGuestToWeddingAsync(weddingId, newGuest.Id);
                await Application.Current.MainPage.DisplayAlert("Success", "New guest added to wedding successfully, we will send an email to the address", "OK");
                SendEmail();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please select an existing user or enter new guest details", "OK");            
            }
            ClearFields();

        }

        private async Task<bool> IsSelectedUserAlreadyAddedAsync(int weddingId, int userId)
        {
            var guest = await _guestService.GetGuestByUserIdAsync(userId);
            if (guest == null)
            {
                return false;
            }
            return await _weddingGuestService.IsGuestAlreadyAddedToWeddingAsync(weddingId, guest.Id);
        }

        private async void SendEmail()
        {
            var Subject = "You have been invited to a wedding!";
            var Body = "We are delighted to invite you to celebrate a special and unique moment in our lives, our wedding. \nPlease visit the app 'the knot' and register with this email to see the details.";

            

            await Microsoft.Maui.ApplicationModel.Communication.Email.Default.ComposeAsync(new Microsoft.Maui.ApplicationModel.Communication.EmailMessage
            {
                Subject = Subject,
                Body = Body,
                To = [NewGuestEmail]
            });
        }

        private void ClearFields()
        {
            NewGuestName = string.Empty;
            NewGuestSurname = string.Empty;
            NewGuestEmail = string.Empty;
            NewGuestPhoneNumber = string.Empty;
            NewGuestDietaryRestrictions = string.Empty;
            SelectedUser = null;
            SearchQuery = string.Empty;
        }
    }
}
