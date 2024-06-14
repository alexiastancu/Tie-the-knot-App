using Wedding_Planning_App.ViewModels;

namespace Wedding_Planning_App.Views;

public partial class SignIn : ContentPage
{
	public SignIn(SignInVM vm)
	{
		InitializeComponent();
        BindingContext = vm;
        //SecureStorage.SetAsync("hasAuth", "true");
    }
}