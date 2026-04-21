using _8osa.Services;

namespace _8osa;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        LanguageService.LoadSavedLanguage();
        MainPage = new NavigationPage(new KarussellPage())
        {
            BarBackgroundColor = Color.FromArgb("#0F0F1A"),
            BarTextColor = Colors.White
        };
    }
}