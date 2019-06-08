using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Tsukinia.Translation
{
    public static class TextProvider
    {
        private static CultureInfo CurrentCulture;

        private static Dictionary<int, int> LanguageMap = new Dictionary<int, int>(){
            {1033, 0},
            {1045, 1}
        };

        public static void SetCulture(CultureInfo cultureInfo)
        {
            CurrentCulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        public static string GetText(string key)
        {
            var languageIndex = LanguageMap.ContainsKey(CurrentCulture.LCID) ? LanguageMap[CurrentCulture.LCID] : LanguageMap.First().Value;

            if (AllText.ContainsKey(key))
            {
                return AllText[key].Texts[languageIndex];
            }

            //TODO: maybe remove, throw exception
            return $"##{key}##";
        }

        private static Dictionary<string, TranslationData> AllText = new Dictionary<string, TranslationData>()
        {
            {"Hello", new TranslationData("Hello,", "Witaj")},
            {"YourBalanceIs", new TranslationData("Your balance is:", "Twój saldo to:")},
            {"Home", new TranslationData("Home","Start")},
            {"Activity", new TranslationData("Activity", "Aktywność")},
            {"Settings", new TranslationData("Settings", "Ustawienia")}
        };

        private class TranslationData
        {
            public string[] Texts { get; private set; }

            public TranslationData(params string[] texts)
            {
                this.Texts = texts;
            }
        }

    }
}