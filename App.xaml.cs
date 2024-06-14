using Wedding_Planning_App.Data;

namespace Wedding_Planning_App
{
    public partial class App : Application
    {
        public App()
        {
            try
            {
                InitializeComponent();
                //UserAppTheme = AppTheme.Light;
                MainPage = new AppShell();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting MainPage: {ex.Message}");
                throw;
            }
        }
    }
}
