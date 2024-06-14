using Wedding_Planning_App.Data;
using Wedding_Planning_App.ViewModels;

namespace Wedding_Planning_App.Views;

public partial class SignUp : ContentPage
{
	public SignUp(SignUpVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}