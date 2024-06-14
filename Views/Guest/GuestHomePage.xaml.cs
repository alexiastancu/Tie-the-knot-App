using Wedding_Planning_App.ViewModels.Guest;

namespace Wedding_Planning_App.Views.Guest;

public partial class GuestHomePage : ContentPage
{
	public GuestHomePage(GuestHomePageVM vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        ((GuestHomePageVM)BindingContext).LoadGuestWeddings();
    }
}