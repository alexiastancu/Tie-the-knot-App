using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Data.Enums;
using Wedding_Planning_App.Models;
using Wedding_Planning_App.Services.Interfaces;

namespace Wedding_Planning_App.ViewModels.Guest
{
    public partial class PendingInvitationsVM : ObservableRecipient
    {
        private readonly IGuestService _guestService;
        private readonly IWeddingGuestService _weddingGuestService;

        [ObservableProperty]
        private ObservableCollection<WeddingGuestIntermediate> pendingInvitations;

        [ObservableProperty]
        private Models.Guest guest;

        public PendingInvitationsVM(IGuestService guestService, IWeddingGuestService weddingGuestService)
        {
            _guestService = guestService;
            _weddingGuestService = weddingGuestService;
            //LoadPendingInvitations();
        }

        public PendingInvitationsVM()
        {
            
        }

        [RelayCommand]
        public async Task LoadPendingInvitations()
        {
            try
            {
                bool conversionSucceded = int.TryParse(await SecureStorage.GetAsync("userId"), out int userId);
                if (!conversionSucceded)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "There was a problem retrieving the user, try again later", "OK");
                    Debug.WriteLine("Error retrieving userId from SecureStorage or converting it to int");
                    return;
                }
                Guest = await _guestService.GetGuestByUserIdAsync(userId);
                var invitations = await _weddingGuestService.GetPendingInvitationsByGuestIdAsync(Guest.Id);
                PendingInvitations = new ObservableCollection<WeddingGuestIntermediate>(invitations);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading pending invitations: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task AcceptInvitation(WeddingGuestIntermediate invitation)
        {
            try
            {
                invitation.InvitationStatus = InvitationStatus.Accepted;
                await _weddingGuestService.UpdateWeddingGuestIntermediateAsync(invitation);
                PendingInvitations.Remove(invitation);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error accepting invitation: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task RejectInvitation(WeddingGuestIntermediate invitation)
        {
            try
            {
                invitation.InvitationStatus = InvitationStatus.Rejected;
                await _weddingGuestService.UpdateWeddingGuestIntermediateAsync(invitation);
                //await _weddingGuestService.RemoveGuestFromWeddingAsync(invitation.WeddingId, invitation.GuestId);
                PendingInvitations.Remove(invitation);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error rejecting invitation: {ex.Message}");
            }
        }
    }
}
