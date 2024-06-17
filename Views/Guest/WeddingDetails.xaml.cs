using Wedding_Planning_App.ViewModels.Guest;

namespace Wedding_Planning_App.Views.Guest;
[QueryProperty(nameof(WeddingId), nameof(WeddingId))]

public partial class WeddingDetails : ContentPage
{
    public int WeddingId { get; set; }
    public WeddingDetails(WeddingDetailsVM vm)
	{
		InitializeComponent();
        BindingContext = vm;
        vm._map = map;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        ((WeddingDetailsVM)BindingContext).WeddingId = WeddingId;
        ((WeddingDetailsVM)BindingContext).LoadWeddingDetails();
    }
}