using Wedding_Planning_App.ViewModels.Fiances;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
namespace Wedding_Planning_App.Views.Fiances;

public partial class AddWedding : ContentPage
{
    public AddWedding(AddWeddingVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
        vm._map = map;
        datePicker.Date = DateTime.Now;

        //InitializeMapPosition();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

    }

    private async void InitializeMapPosition()
    {
        try
        {
            var location = await Geolocation.GetLastKnownLocationAsync();

            if (location == null)
            {
                location = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Best,
                    Timeout = TimeSpan.FromSeconds(30)
                });
            }

            if (location != null)
            {
                var position = new Location(location.Latitude, location.Longitude);
                map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1)));
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions, such as permissions not granted or location services disabled
            await DisplayAlert("Error", "Unable to get location: " + ex.Message, "OK");
        }
    }
}