namespace Wedding_Planning_App.Views.Fiances;
using Wedding_Planning_App.Models;
using Wedding_Planning_App.ViewModels.Fiances;

[QueryProperty(nameof(WeddingId), nameof(WeddingId))]
public partial class GiftList : ContentPage
{
    public int WeddingId { get; set; }
    public GiftList(GiftListVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            var checkBox = (CheckBox)sender;
            var gift = (Gift)checkBox.BindingContext;
            var viewModel = (GiftListVM)BindingContext;

            if (gift == null)
            {
                return;
            }
            // Prevent unchecking
            if (!gift.IsPurchased)
            {
                viewModel.PurchaseGiftCommand.Execute(gift);
                if(viewModel.IsFiance)
                {
                    checkBox.IsChecked = false;
                }
            }
            else
            {
                checkBox.IsChecked = true;
            }
        }
        else
        {
            var checkBox = (CheckBox)sender;
            checkBox.IsChecked = true;
        }
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        ((GiftListVM)BindingContext).WeddingId = WeddingId;
        ((GiftListVM)BindingContext).LoadUserRole();
        ((GiftListVM)BindingContext).LoadGifts();
    }
}