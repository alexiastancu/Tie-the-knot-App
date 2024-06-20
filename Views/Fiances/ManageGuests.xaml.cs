using Wedding_Planning_App.ViewModels.Fiances;

namespace Wedding_Planning_App.Views.Fiances;

public partial class ManageGuests : ContentPage
{
	public ManageGuests(ManageGuestsVM vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    //protected override void OnNavigatedTo(NavigatedToEventArgs args)
    //{
    //    base.OnNavigatedTo(args);
    //    (BindingContext as ManageGuestsVM).LoadGuests();
    //}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as ManageGuestsVM).LoadGuests();
    }
}