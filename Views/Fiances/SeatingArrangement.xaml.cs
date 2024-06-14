using Wedding_Planning_App.ViewModels.Fiances;

namespace Wedding_Planning_App.Views.Fiances;

public partial class SeatingArrangement : ContentPage
{
	public SeatingArrangement(SeatingArrangementVM vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}