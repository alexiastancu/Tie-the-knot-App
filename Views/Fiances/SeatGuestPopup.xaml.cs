using CommunityToolkit.Maui.Views;
using Wedding_Planning_App.Models;
using Wedding_Planning_App.Services.Interfaces;
using Wedding_Planning_App.ViewModels.Fiances;

namespace Wedding_Planning_App.Views.Fiances;

public partial class SeatGuestPopup : Popup
{
	public SeatGuestPopup(GuestSeat guestSeat)
	{
		InitializeComponent();
        BindingContext = new SeatGuestPopupVM(guestSeat, this);
    }
}