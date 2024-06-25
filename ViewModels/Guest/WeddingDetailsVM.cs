using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Map = Microsoft.Maui.Controls.Maps.Map;
using Wedding_Planning_App.Models;
using Wedding_Planning_App.Services.Interfaces;
using Wedding_Planning_App.ViewModels.Fiances;
using Wedding_Planning_App.Views.Fiances;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Diagnostics;
using System.Globalization;

namespace Wedding_Planning_App.ViewModels.Guest
{
    [QueryProperty(nameof(WeddingId), nameof(WeddingId))]
    public partial class WeddingDetailsVM : ObservableRecipient
    {
        [ObservableProperty]
        public Map _map;

        [ObservableProperty]
        private Models.Location location;

        [ObservableProperty]
        private int weddingId;

        [ObservableProperty]
        private Wedding wedding;

        [ObservableProperty]
        private GuestSeat guestSeat;

        [ObservableProperty]
        private WeddingTable guestTable;

        [ObservableProperty]
        private bool isSeatAssigned;

        [ObservableProperty]
        private int selectedSeatIndex;

        private readonly IWeddingService _weddingService;
        private readonly IWeddingTableService _weddingTableService;
        private readonly IGuestSeatService _guestSeatService;
        private readonly IGuestService _guestService;
        private readonly ILocationService _locationService;

        public WeddingDetailsVM(IWeddingService weddingService, IGuestSeatService guestSeatService, IGuestService guestService, IWeddingTableService weddingTableService, ILocationService locationService)
        {
            _weddingService = weddingService;
            _guestSeatService = guestSeatService;
            _guestService = guestService;
            _weddingTableService = weddingTableService;
            _locationService = locationService;
        }
        public WeddingDetailsVM()
        {

        }

        [RelayCommand]
        public async Task LoadWeddingDetails()
        {
            Wedding = await _weddingService.GetWeddingByIdAsync(WeddingId);
            if (Wedding != null)
            {
                Location = await _locationService.GetLocationById(Wedding.LocationId);
                Map.Pins.Add(new Pin
                {
                    Label = "Wedding location",
                    Location = new Microsoft.Maui.Devices.Sensors.Location(Location.Latitude, Location.Longitude),
                    Type = PinType.Place
                });
                Map.MoveToRegion(new MapSpan(Map.Pins.First().Location, 0.01, 0.01));
            }
            await LoadGuestSeatDetails();
        }

        private async Task LoadGuestSeatDetails()
        {
            bool conversionSucceded = int.TryParse(await SecureStorage.GetAsync("userId"), out int userId);
            if (!conversionSucceded)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "There was a problem retrieving the user, try again later", "OK");
                Debug.WriteLine("Error retrieving userId from SecureStorage or converting it to int");
                return;
            }

            var guest = await _guestService.GetGuestByUserIdAsync(userId);
            if (guest != null)
            {
                GuestSeat = await _guestSeatService.GetGuestSeatByGuestIdAsync(guest.Id, wedding.Id);
                if (GuestSeat != null)
                {
                    GuestTable = await _weddingTableService.GetTableByIdAsync(GuestSeat.TableId);
                    SelectedSeatIndex = GuestTable.Seats.IndexOf(GuestTable.Seats.FirstOrDefault(seat => seat.Id == GuestSeat.Id));
                    IsSeatAssigned = true;
                }
            }

        }

        [RelayCommand]
        private async Task OpenGoogleMaps()
        {
            if (Location != null)
            {
                var lat = Location.Latitude.ToString(CultureInfo.InvariantCulture);
                var lng = Location.Longitude.ToString(CultureInfo.InvariantCulture);
                var uri = new Uri($"https://www.google.com/maps/search/?api=1&query={lat} {lng}");
                await Launcher.Default.OpenAsync(uri);
            }
        }

        [RelayCommand]
        public async Task NavigateToGiftList()
        {
            await Shell.Current.GoToAsync($"{nameof(GiftList)}?{nameof(GiftListVM.WeddingId)}={Wedding.Id}");
        }
    }
}
