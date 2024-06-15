using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Models;
using Wedding_Planning_App.Services;
using BCrypt.Net;
using Wedding_Planning_App.Data.Enums;
using Task = System.Threading.Tasks.Task;
using Wedding_Planning_App.ViewModels.Fiances;
using Wedding_Planning_App.Views.Fiances;
using Wedding_Planning_App.Data;
using System.Data.Common;
using DbConnection = Wedding_Planning_App.Data.DbConnection;
using System.Diagnostics;
using Wedding_Planning_App.Views.Vendor;
using Wedding_Planning_App.Views.Admin;
using Wedding_Planning_App.Views.Guest;
using Wedding_Planning_App.Services.Interfaces;

namespace Wedding_Planning_App.ViewModels
{
    public partial class SignInVM : ObservableValidator
    {
        private readonly IUserService _userService;


        [ObservableProperty, Required(ErrorMessage = "Email is required")]
        private string _email;
        [ObservableProperty, Required(ErrorMessage = "Password is required")]
        private string _password;


        public SignInVM()
        {
            _userService = new UserService();
        }


        [RelayCommand]
        public async Task<bool> SignIn()
        {
            if (!ValidateProperties())
                return false;

            var user = await _userService.FindByEmailAsync(Email);
            if (user == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "User not found", "OK");
                return false;
            }

            if(user.PasswordHash == null)
            {
                await Application.Current.MainPage.DisplayAlert("Warning", "Let's complete your account!", "OK");

                return false;
            }
            var passwordCorrect = BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash);
            if (!passwordCorrect)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Incorrect password", "OK");
                return false;
            }
            await SecureStorage.SetAsync("hasAuth", "true");
            await SecureStorage.SetAsync("userId", user.Id.ToString());
            ((AppShell)Application.Current.MainPage).OnLoginStatusChanged(user.Role);
            switch (user.Role)
            {
                case UserRoles.Fiancés:
                    int hasWedding = await _userService.UserHasWeddingAsync(user);
                    if (hasWedding != 0)
                    {
                        await SecureStorage.SetAsync("weddingId", hasWedding.ToString());
                        //await Shell.Current.GoToAsync(nameof(FiancesHomePage),
                        //    new Dictionary<string, object>
                        //    {
                        //        ["User"] = user
                        //    });
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Welcome!", "Let's add your wedding!", "OK");
                        await Shell.Current.GoToAsync(nameof(AddWedding),
                            new Dictionary<string, object>
                            {
                                ["User"] = user
                            });
                    }
                    break;
                case UserRoles.Vendor:
                    //await Shell.Current.GoToAsync(nameof(VendorHomePage),
                    //        new Dictionary<string, object>
                    //        {
                    //            ["User"] = user
                    //        });
                    break;
                case UserRoles.Admin:
                    //await Shell.Current.GoToAsync(nameof(AdminHomePage),
                    //        new Dictionary<string, object>
                    //        {
                    //            ["User"] = user
                    //        });
                    break;
                case UserRoles.Guest:
                    await Shell.Current.GoToAsync(nameof(GuestHomePage),
                            new Dictionary<string, object>
                            {
                                ["User"] = user
                            });
                    break;
            }

            return true;
        }

        [RelayCommand]
        private async void CompleteRegistration()
        {
            await Shell.Current.GoToAsync(nameof(CompleteRegistration));
        }

        private bool ValidateProperties()
        {
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(this, new ValidationContext(this), validationResults, validateAllProperties: true);

            if (!isValid)
            {
                StringBuilder errorMessage = new StringBuilder();
                foreach (var validationResult in validationResults)
                {
                    errorMessage.AppendLine(validationResult.ErrorMessage);
                }

                Application.Current.MainPage.DisplayAlert("Error", errorMessage.ToString(), "OK");
                return false;
            }
            return true;

        }

        [RelayCommand]
        private async void SignUp()
        {
            await Shell.Current.GoToAsync(nameof(SignUp), true);
        }
    }
}
