using Wedding_Planning_App.Data.Enums;
using Wedding_Planning_App.Models;
using Wedding_Planning_App.Services;
using Wedding_Planning_App.Views.Admin;
using Wedding_Planning_App.Views.Fiances;
using Wedding_Planning_App.Views.Guest;
using Wedding_Planning_App.Views.Vendor;
using Microsoft.Maui.Controls;
using Task = System.Threading.Tasks.Task;
using Wedding_Planning_App.Services.Interfaces;
using System.Diagnostics;

namespace Wedding_Planning_App.Views
{
    public partial class LoadingPage : ContentPage
    {
        private readonly IUserService _userService;

        public LoadingPage()
        {
            InitializeComponent();
            _userService = new UserService();
            //Application.Current.UserAppTheme = AppTheme.Light;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);


            var hasAuth = await isAuthenticated();
            if (hasAuth == "true")
            {
                var userId = await SecureStorage.GetAsync("userId");

                User user = await _userService.GetUserById(int.Parse(userId));

                ((AppShell)Application.Current.MainPage).OnLoginStatusChanged(user.Role);

                switch (user?.Role)
                {
                    case UserRoles.Fiancés:
                        int hasWedding = await _userService.UserHasWeddingAsync(user);
                        if (hasWedding != 0)
                        {
                            await SecureStorage.SetAsync("weddingId", hasWedding.ToString());
                            //await NavigateToPage(nameof(FiancesHomePage), user);
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Welcome!", "Let's add your wedding!", "OK");
                            await NavigateToPage(nameof(AddWedding), user);
                        }
                        break;
                    case UserRoles.Vendor:
                        //await NavigateToPage(nameof(VendorHomePage), user);
                        break;
                    case UserRoles.Admin:
                        //await NavigateToPage(nameof(AdminHomePage), user);
                        break;
                    case UserRoles.Guest:
                        await NavigateToPage(nameof(GuestHomePage), user);
                        break;
                }
            }
            else
            {
                await Shell.Current?.GoToAsync($"//{nameof(SignIn)}", true);
            }
        }

        private async Task NavigateToPage(string pageName, User user)
        {
            if (string.IsNullOrEmpty(pageName))
            {
                throw new ArgumentNullException(nameof(pageName), "Page name cannot be null or empty.");
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }
            try
            {
                await Shell.Current.GoToAsync($"/{pageName}", true,
                    new Dictionary<string, object>
                    {
                        ["User"] = user
                    });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Debug.WriteLine($"Navigation Exception: {ex.Message}");
            }

        }

        private async Task<string> isAuthenticated()
        {
            try
            {
                //await SecureStorage.SetAsync("hasAuth", "false");

                await Task.Delay(1000);
                var hasAuth = await SecureStorage.GetAsync("hasAuth");
                return hasAuth;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Debug.WriteLine($"Authentication Exception: {ex.Message}");
                return null;
            }
        }
    }
}
