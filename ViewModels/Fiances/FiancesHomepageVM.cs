using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Wedding_Planning_App.Models;
using Wedding_Planning_App.Services;
using Wedding_Planning_App.Services.Interfaces;

namespace Wedding_Planning_App.ViewModels.Fiances
{
    [QueryProperty(nameof(User), nameof(User))]
    public partial class FiancesHomepageVM : ObservableRecipient
    {
        private System.Timers.Timer _timer;

        private readonly IWeddingService _weddingService;

        #region Properties
        [ObservableProperty]
        private User user;

        [ObservableProperty]
        private DateTime weddingDate;

        [ObservableProperty]
        private int years;

        [ObservableProperty]
        private int months;

        [ObservableProperty]
        private int weeks;

        [ObservableProperty]
        private int days;
        #endregion

        public FiancesHomepageVM()
        {
            _weddingService = new WeddingService();

            StartCountdown();
        }

        public async void StartCountdown()
        {
            bool conversionSucceded = int.TryParse(await SecureStorage.GetAsync("weddingId"), out int weddingId);
            if (!conversionSucceded)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "There was a problem retrieving the wedding, try again later", "OK");
                Debug.WriteLine("Error retrieving weddingId from SecureStorage or converting it to int");
                return;
            }
            WeddingDate = (await _weddingService.GetWeddingByIdAsync(weddingId)).WeddingDate;

            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            var now = DateTime.Now;
            var timeSpan = WeddingDate - now;
            if (timeSpan.TotalSeconds > 0)
            {
                var weddingDate = WeddingDate;

                Years = weddingDate.Year - now.Year;
                if (weddingDate.Month < now.Month || (weddingDate.Month == now.Month && weddingDate.Day < now.Day))
                {
                    Years--;
                }

                var targetDate = now.AddYears(Years);
                Months = 0;
                while (targetDate.AddMonths(Months + 1) <= weddingDate)
                {
                    Months++;
                }

                targetDate = targetDate.AddMonths(Months);
                Weeks = (weddingDate - targetDate).Days / 7;
                Days = (weddingDate - targetDate).Days % 7;
            }
            else
            {
                Years = 0;
                Months = 0;
                Weeks = 0;
                Days = 0;
                _timer.Stop();
            }
        }
    }
}
