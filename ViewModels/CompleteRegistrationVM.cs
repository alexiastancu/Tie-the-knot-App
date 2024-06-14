using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Models;
using Wedding_Planning_App.Services.Interfaces;
using Wedding_Planning_App.Views;

namespace Wedding_Planning_App.ViewModels
{
    public partial class CompleteRegistrationVM : ObservableValidator
    {
        private readonly IUserService _userService;
        private readonly IGuestService _guestService;

        #region Properties

        [ObservableProperty]
        private User user;

        [ObservableProperty]
        private Models.Guest guest;

        [ObservableProperty]
        [Required(ErrorMessage = "Name is required")]
        private string name;

        [ObservableProperty]
        [Required(ErrorMessage = "Surname is required")]
        private string surname;

        [ObservableProperty]
        [Required(ErrorMessage = "Email is required")]
        private string email;

        [ObservableProperty]
        [Required(ErrorMessage = "Phone number is required")]
        private string phoneNumber;

        [ObservableProperty]
        private string dietaryRestrictions;

        [ObservableProperty]
        [Required(ErrorMessage = "Password is required")]
        private string password;

        #endregion

        public CompleteRegistrationVM(IUserService userService, IGuestService guestService)
        {
            _userService = userService;
            _guestService = guestService;
        }

        [RelayCommand]
        public async System.Threading.Tasks.Task SearchEmail()
        {
            User = await _userService.FindByEmailAsync(Email);
            if (User != null)
            {
                Name = User.Name;
                Surname = User.Surname;
                PhoneNumber = User.PhoneNumber;
                Guest = await _guestService.GetGuestByUserIdAsync(User.Id);
                DietaryRestrictions = Guest.DietaryRestrictions;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Email not found", "OK");
            }
        }

        [RelayCommand]
        public async System.Threading.Tasks.Task CompleteRegistration()
        {
            #region Validate Properties
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(this, new ValidationContext(this), validationResults, validateAllProperties: true);

            if (!isValid)
            {
                StringBuilder errorMessage = new StringBuilder();
                foreach (var validationResult in validationResults)
                {
                    errorMessage.AppendLine(validationResult.ErrorMessage);
                }

                await Application.Current.MainPage.DisplayAlert("Error", errorMessage.ToString(), "OK");
                return;
            }
            #endregion

            User.Name = Name;
            User.Surname = Surname;
            User.PhoneNumber = PhoneNumber;
            Guest.DietaryRestrictions = DietaryRestrictions;

            int resultUser = await _userService.AddPasswordToUser(user, password);
            int resultGuest = await _guestService.UpdateGuestAsync(Guest);

            if (resultUser > 0 && resultGuest > 0)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Registration completed successfully", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to complete registration", "OK");
            }

        }

    }
}
