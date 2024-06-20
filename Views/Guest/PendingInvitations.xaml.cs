using Wedding_Planning_App.ViewModels.Guest;

namespace Wedding_Planning_App.Views.Guest;

public partial class PendingInvitations : ContentPage
{
	public PendingInvitations(PendingInvitationsVM vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await (BindingContext as PendingInvitationsVM).LoadPendingInvitations();
    }
}