using Syncfusion.Maui.Buttons;
using Syncfusion.Maui.Core.Carousel;
using Wedding_Planning_App.ViewModels.Fiances;

namespace Wedding_Planning_App.Views.Fiances;

public partial class AddGuest : ContentPage
{
    private AddGuestVM vm;
    public AddGuest(AddGuestVM vm)
	{
		InitializeComponent();
        BindingContext = vm;
        this.vm = vm;
    }

    private void OnSegmentSelectionChanged(object sender, EventArgs e)
    {
        var segmentedControl = sender as SfSegmentedControl;
        if (segmentedControl == null) return;

        var selectedIndex = segmentedControl.SelectedIndex;

        if (selectedIndex == 0) // Add Existing Guest
        {
            existingUserSection.IsVisible = true;
            newGuestSection.IsVisible = false;

            // Clear new guest fields
            vm.NewGuestName = null;
            vm.NewGuestEmail = null;
            vm.NewGuestDietaryRestrictions = null;
        }
        else if (selectedIndex == 1) // Add New Guest
        {
            existingUserSection.IsVisible = false;
            newGuestSection.IsVisible = true;

            // Clear existing user selection
            vm.SelectedUser = null;
            vm.SearchQuery = null;
        }
    }
}