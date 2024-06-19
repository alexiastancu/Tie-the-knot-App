using Wedding_Planning_App.ViewModels.Fiances;

namespace Wedding_Planning_App.Views.Fiances;

public partial class ManageGuests : ContentPage
{
	public ManageGuests(ManageGuestsVM vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}