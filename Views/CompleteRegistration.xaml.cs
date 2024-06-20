using Wedding_Planning_App.ViewModels;

namespace Wedding_Planning_App.Views;

public partial class CompleteRegistration : ContentPage
{
    public CompleteRegistration(CompleteRegistrationVM vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    
}