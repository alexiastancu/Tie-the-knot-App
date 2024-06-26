using Wedding_Planning_App.ViewModels.Fiances;

namespace Wedding_Planning_App.Views.Fiances;

public partial class FiancesHomePage : ContentPage
{
    public FiancesHomePage(FiancesHomepageVM vm)
    {
        InitializeComponent();
        //BindingContext = new FiancesHomepageVM();
        BindingContext = vm;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        //(BindingContext as FiancesHomepageVM).StartCountdown();

    }
}