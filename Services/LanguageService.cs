using System.Globalization;

namespace _8osa.Services
{
    public static class LanguageService
    {
        public static event Action? LanguageChanged;

        private static string _currentLanguage = "en";
        public static string CurrentLanguage => _currentLanguage;

        public static void ChangeLanguage(string languageCode)
        {
            _currentLanguage = languageCode;
            var culture = new CultureInfo(languageCode);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            Preferences.Set("AppLanguage", languageCode);
            LanguageChanged?.Invoke();
        }

        public static void LoadSavedLanguage()
        {
            var saved = Preferences.Get("AppLanguage", "en");
            ChangeLanguage(saved);
        }
    }
}