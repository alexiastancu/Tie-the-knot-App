using Microsoft.Extensions.Logging;
using Wedding_Planning_App.Data;
using Microsoft.Extensions.DependencyInjection;
using Wedding_Planning_App.Models;
using Wedding_Planning_App.ViewModels;
using SQLite;
using Task = Wedding_Planning_App.Models.Task;
using MenuItem = Wedding_Planning_App.Models.MenuItem;
using Wedding_Planning_App.Services;
using Wedding_Planning_App.Views;
using Microsoft.Maui.ApplicationModel;
using Wedding_Planning_App.Views.Fiances;
using Wedding_Planning_App.ViewModels.Fiances;
using Wedding_Planning_App.Services.Interfaces;
using Syncfusion.Maui.Core.Hosting;
using Wedding_Planning_App.Views.Guest;
using Wedding_Planning_App.ViewModels.Guest;

namespace Wedding_Planning_App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("SF-Pro.ttf", "SFPro");
                    fonts.AddFont("SF-Pro-Text-Bold.otf", "SFProBold");
                })
                .ConfigureSyncfusionCore()
            .UseMauiMaps();
            builder.Services.AddSingleton<DbConnection>();

            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IWeddingService, WeddingService>();
            builder.Services.AddSingleton<ILocationService, LocationService>();
            builder.Services.AddSingleton<IWeddingGuestService, WeddingGuestService>();
            builder.Services.AddSingleton<IGuestService, GuestService>();
            builder.Services.AddSingleton<IFiancesService, FiancesService>();


            builder.Services.AddTransient<SignIn>();
            builder.Services.AddTransient<SignUp>();
            builder.Services.AddTransient<AddWedding>();
            builder.Services.AddTransient<FiancesHomePage>();
            builder.Services.AddTransient<GuestHomePage>();
            builder.Services.AddTransient<AddGuest>();
            builder.Services.AddTransient<CompleteRegistration>();
            builder.Services.AddTransient<SeatingArrangement>();


            builder.Services.AddSingleton<SignUpVM>();
            builder.Services.AddSingleton<SignInVM>();
            builder.Services.AddSingleton<AddWeddingVM>();
            builder.Services.AddSingleton<FiancesHomepageVM>();
            builder.Services.AddSingleton<GuestHomePageVM>();
            builder.Services.AddTransient<AddGuestVM>();
            builder.Services.AddSingleton<CompleteRegistrationVM>();
            builder.Services.AddSingleton<SeatingArrangementVM>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        
    }
}
