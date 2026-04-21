using _8osa.Services;

namespace _8osa;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        LanguageService.LoadSaved();
        MainPage = new KarussellPage();
    }
}