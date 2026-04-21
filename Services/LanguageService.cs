using _8osa.Resources.Localization;
using System.Globalization;

namespace _8osa.Services
{
    public static class LanguageService
    {
        public static event Action? LanguageChanged;

        public static void ChangeLanguage(string languageCode)
        {
            var culture = new CultureInfo(languageCode);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            AppResources.Culture = culture;

            Preferences.Set("AppLanguage", languageCode);
            LanguageChanged?.Invoke();
        }

        public static void LoadSaved()
        {
            var saved = Preferences.Get("AppLanguage", "en");
            ChangeLanguage(saved);
        }
    }
}