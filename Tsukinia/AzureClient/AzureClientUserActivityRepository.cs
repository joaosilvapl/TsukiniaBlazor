using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Tsukinia.Core;

namespace Tsukinia.AzureClient
{
    public class AzureClientUserActivityRepository
    {
        public async Task<DataOperationResult<UserActivityData>> Insert(UserActivityData userActivityData)
        {
            return await AzureClientHelper.PostData<DataOperationResult<UserActivityData>>("AddUserActivityFunction", new
            {
                userId = userActivityData.UserId,
                activityId = userActivityData.ActivityId,
                userActivityType = userActivityData.ActivityType.ToString(),
                activityTitle = userActivityData.ActivityTitle,
                activityComments = userActivityData.ActivityComments,
                points = userActivityData.Points,
                dateUtc = userActivityData.DateUtc.ToString("o", CultureInfo.InvariantCulture)
            });
        }

        public async Task<DataOperationResult<List<UserActivityData>>> GetAllForUser(string userId)
        {
            return await AzureClientHelper.GetData<DataOperationResult<List<UserActivityData>>>(
                $"GetAllUserActivityFunction?userId={userId.ToString()}");
        }
    }
}
