using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Data;
using Wedding_Planning_App.Data.Enums;
using Wedding_Planning_App.Models;
using Wedding_Planning_App.Services;
using Wedding_Planning_App.Services.Interfaces;
using Wedding_Planning_App.Views;

namespace Wedding_Planning_App.ViewModels
{
    public partial class SignUpVM : ObservableValidator
    {
        private readonly IUserService _userService;
        private readonly IGuestService _guestService;
        private readonly IFiancesService _fiancesService;
        private DbConnection _context => new DbConnection();


        #region Properties
        [ObservableProperty, Required(ErrorMessage = "Name is required")]
        public string _name;

        [ObservableProperty, Required(ErrorMessage = "Surname is required")]
        public string _surname;

        [ObservableProperty, Required(ErrorMessage = "Email is required")]
        public string _email;

        [ObservableProperty, Required(ErrorMessage = "Password is required")]
        public string _password;

        [ObservableProperty, Required(ErrorMessage = "Phone number is required")]
        public string _phoneNumber;

        [ObservableProperty]
        public string _serviceDescription;

        [ObservableProperty]
        public string _category;

        [ObservableProperty]
        public string _dietaryRestrictions;

        [ObservableProperty]
        public string _husbandName;

        [ObservableProperty]
        public string _husbandSurname;

        [ObservableProperty]
        public string _wifeName;

        [ObservableProperty]
        public string _wifeSurname;

        #endregion

        #region Roles
        private string _selectedRole;
        [Required(ErrorMessage = "Role is required")]
        public string SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
                UpdateRoleSpecificFieldsVisibility();
            }
        }


        private void UpdateRoleSpecificFieldsVisibility()
        {
            // Show/Hide Role-Specific Fields based on the selected role
            ServiceDescriptionIsVisible = SelectedRole == "Vendor";
            CategoryIsVisible = SelectedRole == "Vendor";
            DietaryRestrictionsIsVisible = SelectedRole == "Guest";
            HusbandNameIsVisible = SelectedRole == "Fiancés";
        }

        [ObservableProperty]
        private bool _serviceDescriptionIsVisible;
        [ObservableProperty]
        private bool _categoryIsVisible;
        [ObservableProperty]
        private bool _dietaryRestrictionsIsVisible;
        [ObservableProperty]
        private bool _husbandNameIsVisible;

        #endregion


        public SignUpVM()
        {
            _userService = new UserService();
            _guestService = new GuestService();
            _fiancesService = new FiancesService();
        }

        [RelayCommand]
        public async Task<bool> SignUp(object parameter)
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
                return false;
            }
            #endregion

            var user = await _userService.FindByEmailAsync(Email);
            if (user != null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "This email address is already in use", "OK");
                return false;
            }
            var newUser = new User
            {
                Name = Name,
                Surname = Surname,
                //UserName = Username,
                Email = Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password),
                PhoneNumber = PhoneNumber,
                Role = (UserRoles)Enum.Parse(typeof(UserRoles), SelectedRole)
            };
            var result = await _userService.CreateAsync(newUser, Password);
            if (result > 0)
            {
                int userId = newUser.Id; // Assuming the Id is set after the user is created

                switch (SelectedRole)
                {
                    case "Guest":
                        var guest = new Models.Guest
                        {
                            UserId = userId,
                            DietaryRestrictions = DietaryRestrictions
                        };
                        await _guestService.AddGuestAsync(guest);
                        break;
                    case "Fiancés":
                        var fiances = new Models.Fiances
                        {
                            UserId = userId,
                            HusbandName = HusbandName,
                            WifeName = WifeName,
                            HusbandSurname = HusbandSurname,
                            WifeSurname = WifeSurname
                        };
                        await _fiancesService.AddFiancesAsync(fiances);
                        break;
                }
                await Application.Current.MainPage.DisplayAlert("Success", "User created successfully", "OK");
                await Shell.Current.GoToAsync("///SignIn");
            }
            else
            {
                Debug.WriteLine("Couldn't add the user to the database, sorry!");
            }
            return false;
        }

        [RelayCommand]
        private async void SignIn()
        {
            await Shell.Current.GoToAsync(nameof(SignIn), true);

        }
    }
}
