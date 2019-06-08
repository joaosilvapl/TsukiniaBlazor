using System.Collections.Generic;

namespace Tsukinia.Core
{
    public class AppData
    {
        public AppState AppState { get; set; }

        public UserData UserData { get; set; }

        public UserData ChildData { get; set; }

        public List<UserActivityData> ChildActivities { get; set; }

        public string CurrencySymbol { get; set; }
    }

}
