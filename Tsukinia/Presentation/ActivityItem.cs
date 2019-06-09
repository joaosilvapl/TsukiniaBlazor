using Tsukinia.Core;

namespace Tsukinia.Presentation
{
    public class ActivityItem
    {
        public string IconClass { get; }
        public string Title { get; }
        public int Points { get; }

        public ActivityItem(UserActivityData userActivityData)
        {
            this.Title = userActivityData.ActivityTitle;
            this.Points = userActivityData.Points;
            this.IconClass = this.GetIconClass(userActivityData.ActivityType);
        }

        private string GetIconClass(ActivityType activityType)
        {
            switch (activityType)
            {
                case ActivityType.ManualAddOrRemove:

                    if (this.Points >= 0)
                    {
                        return "oi-check activity_icon_blue";
                    }
                    else
                    {
                        return "oi-bug activity_icon_red";
                    }

                case ActivityType.SchoolTestResult:
                    return "oi-pencil activity_icon_purple";

                case ActivityType.Withdrawal:
                    return "oi-transfer activity_icon_green";

                default:
                    return "";
            }
        }
    }
}