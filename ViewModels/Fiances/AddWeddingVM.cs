using CommunityToolkit.Mvvm.ComponentModel;
using Wedding_Planning_App.Models;
using Wedding_Planning_App.Data.Enums;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;
using System.Net;
using System.Text.Json;
using Wedding_Planning_App.Data;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Diagnostics.Metrics;
using Microsoft.Maui.Maps;
using System;
using System.IO;
using System.Diagnostics;
using Wedding_Planning_App.Views.Fiances;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Wedding_Planning_App.Services.Interfaces;

namespace Wedding_Planning_App.ViewModels.Fiances
{
    [QueryProperty(nameof(User), nameof(User))]
    public partial class AddWeddingVM : ObservableValidator
    {
        private readonly IFiancesService _fiancesService;
        private readonly IWeddingService _weddingService;
        private readonly ILocationService _locationService;
        [ObservableProperty]
        private Models.User user;

        #region Properties
        [ObservableProperty]
        private DateTime weddingDate;

        [ObservableProperty]
        private string budget;

        private Models.Location location;

        [ObservableProperty]
        private string numberOfGuests;

        [ObservableProperty]
        private string theme;

        [ObservableProperty]
        private string selectedAttire;
        #endregion

        public AddWeddingVM(IWeddingService weddingService, ILocationService locationService, IFiancesService fiancesService)
        {
            _weddingService = weddingService;
            _locationService = locationService;
            _fiancesService = fiancesService;
        }
        [ObservableProperty]
        public Map _map;

        [RelayCommand]
        public async System.Threading.Tasks.Task SearchLocation(string entry)
        {
            if (string.IsNullOrWhiteSpace(entry))
                return;

            location = await GetLocation(entry);
            if (location == null)
                return;
            await _locationService.CreateLocationAsync(location);

            Map.Pins.Clear();
            Map.Pins.Add(new Pin
            {
                Label = entry,
                Location = new Microsoft.Maui.Devices.Sensors.Location(location.Latitude, location.Longitude),
                Type = PinType.Place
            });
            Map.MoveToRegion(new MapSpan(Map.Pins.First().Location, 0.01, 0.01));
        }

        private async Task<Models.Location?> GetLocation(string entry)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No internet connection available. Please check your network settings.", "OK");
                return null;
            }
            try
            {
                var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(entry)}&key={Constants.API_KEY}";
                using var client = new HttpClient();
                var response = await client.GetStringAsync(url);
                var data = JObject.Parse(response);
                var location = data["results"]?[0]?["geometry"]?["location"];
                var addressComponents = data["results"]?[0]?["address_components"];

                return new Models.Location
                {
                    Latitude = double.Parse(location?["lat"]?.ToString()),
                    Longitude = double.Parse(location?["lng"]?.ToString()),
                    StreetNumber = GetAddressComponent(addressComponents, "street_number"),
                    Street = GetAddressComponent(addressComponents, "route"),
                    City = GetAddressComponent(addressComponents, "locality"),
                    State = GetAddressComponent(addressComponents, "administrative_area_level_1"),
                    Country = GetAddressComponent(addressComponents, "country"),
                    //FormattedAddress = string.Join(", ", new string[] { Street, StreetNumber, City, State, Country })
                };
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Unable to get location: {ex.Message}", "OK");
                return null;
            }
        }

        private string GetAddressComponent(JToken addressComponents, string type)
        {
            foreach (var component in addressComponents)
            {
                var types = component["types"];
                if (types != null && types.Type == JTokenType.Array)
                {
                    foreach (var t in types)
                    {
                        if (t.ToString() == type)
                        {
                            return component["long_name"]?.ToString();
                        }
                    }
                }
            }
            return string.Empty;
        }

        [RelayCommand]
        public async System.Threading.Tasks.Task AddWedding()
        {
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(this, new ValidationContext(this), validationResults, validateAllProperties: true);

            if (!isValid)
            {
                StringBuilder errorMessage = new StringBuilder();
                foreach (var validationResult in validationResults)
                {
                    errorMessage.AppendLine(validationResult.ErrorMessage);
                }

                await Application.Current.MainPage.DisplayAlert("Error", errorMessage.ToString(), "OK");
                return;
            }
            var fiances = await _fiancesService.GetFiancesByUserIdAsync(User.Id);
            var wedding = new Wedding
            {
                UserId = User.Id,
                FiancesId = fiances.Id,
                WeddingDate = WeddingDate,
                Budget = decimal.Parse(Budget),
                LocationId = location.LocationID,
                EstimatedGuestCount = int.Parse(NumberOfGuests),
                WeddingAttire = (WeddingAttire)Enum.Parse(typeof(WeddingAttire), SelectedAttire)
            };

            var weddingId = await _weddingService.AddWeddingAsync(wedding);
            await SecureStorage.SetAsync("weddingId", weddingId.ToString());

            await Application.Current.MainPage.DisplayAlert("Success", "Wedding added successfully", "OK");
            ((AppShell)Application.Current.MainPage).OnLoginStatusChanged(User.Role);
            await Shell.Current.GoToAsync("..");

        }
    }
}
