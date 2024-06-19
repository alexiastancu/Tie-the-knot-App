using Wedding_Planning_App.ViewModels;

namespace Wedding_Planning_App.Views;

public partial class Settings : ContentPage
{
	public Settings(SettingsVM vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}