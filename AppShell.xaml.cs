using Wedding_Planning_App.Data.Enums;
using Wedding_Planning_App.ViewModels.Fiances;
using Wedding_Planning_App.Views;
using Wedding_Planning_App.Views.Admin;
using Wedding_Planning_App.Views.Fiances;
using Wedding_Planning_App.Views.Guest;
using Wedding_Planning_App.Views.Vendor;

namespace Wedding_Planning_App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(SignIn), typeof(SignIn));
            Routing.RegisterRoute(nameof(SignUp), typeof(SignUp));
            Routing.RegisterRoute(nameof(CompleteRegistration), typeof(CompleteRegistration));
            Routing.RegisterRoute(nameof(AddWedding), typeof(AddWedding));
            Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
            Routing.RegisterRoute(nameof(FiancesHomePage), typeof(FiancesHomePage));
            Routing.RegisterRoute(nameof(AdminHomePage), typeof(AdminHomePage));
            Routing.RegisterRoute(nameof(GuestHomePage), typeof(GuestHomePage));
            Routing.RegisterRoute(nameof(VendorHomePage), typeof(VendorHomePage));
            Routing.RegisterRoute(nameof(WeddingDetails), typeof(WeddingDetails));
            Routing.RegisterRoute(nameof(GiftList), typeof(GiftList));
            Routing.RegisterRoute(nameof(Settings), typeof(Settings));

            Routing.RegisterRoute("//LoadingPage/SignIn", typeof(SignIn));

        }

        public async void SetupFlyoutItems(UserRoles userRole)
        {
            Items.Clear();

            switch (userRole)
            {
                case UserRoles.Fiancés:
                    AddFlyoutItem("Home", "home.png", typeof(FiancesHomePage));
                    AddFlyoutItem("Add Guests", "add_guests.png", typeof(AddGuest));
                    AddFlyoutItem("Seat Guests", "seat.png", typeof(SeatingArrangement));
                    AddFlyoutItem("View Guests", "guests.png", typeof(ManageGuests));
                    AddFlyoutItem("Gift List", "gift.png", typeof(GiftList));
                    break;
                case UserRoles.Vendor:
                    AddFlyoutItem("Home", "home.png", typeof(VendorHomePage));
                    break;
                case UserRoles.Admin:
                    AddFlyoutItem("Home", "home.png", typeof(AdminHomePage));
                    break;
                case UserRoles.Guest:
                    AddFlyoutItem("Home", "home.png", typeof(GuestHomePage));
                    break;
                default:
                    break;
            }

            var logoutItem = new MenuItem
            {
                Text = "Logout",
                IconImageSource = "logout_icon.png"
            };
            logoutItem.Clicked += OnLogoutClicked;

            this.Items.Add(logoutItem);

            var settingsItem = new MenuItem
            {
                Text = "Settings",
                IconImageSource = "settings.png"
            };
            settingsItem.Clicked += OnSettingsClicked;
            this.Items.Add(settingsItem);

        }

        private void AddFlyoutItem(string title, string icon, Type pageType)
        {
            var flyoutItem = new FlyoutItem
            {
                Title = title,
                Icon = icon
            };
            flyoutItem.Items.Add(new ShellContent
            {
                ContentTemplate = new DataTemplate(pageType)
            });
            this.Items.Add(flyoutItem);
        }

        private async void SetupFlyout()
        {            
            var hasAuth = await SecureStorage.GetAsync("hasAuth");
            var isLoggedIn = bool.Parse(hasAuth ?? "false");

            FlyoutBehavior = isLoggedIn ? FlyoutBehavior.Flyout : FlyoutBehavior.Disabled;
        }

        public void OnLoginStatusChanged(UserRoles userRole)
        {
            SetupFlyout();
            if (FlyoutBehavior == FlyoutBehavior.Flyout)
            {
                //FlyoutIcon = "menu_icon.png";
                SetupFlyoutItems(userRole);

            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            await SecureStorage.SetAsync("hasAuth", "false");
            SecureStorage.Remove("userId");
            SecureStorage.Remove("weddingId");

            await Shell.Current.GoToAsync(nameof(SignIn), true);

        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Settings), true);
        }

    }
}
