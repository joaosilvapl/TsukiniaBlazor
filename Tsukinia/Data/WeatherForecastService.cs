using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Tsukinia.Core;

namespace Tsukinia.Data
{
    public class WeatherForecastService
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public async Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {

            // HttpClient client = new HttpClient();

            // var url = Tsukinia.Secrets.GetUserByIdFunctionUrl.Replace("{userId}", "47b63af8-2c2e-4fe8-a438-ff221fbce180");

            // var dataAsString = await client.GetAsync(url);

            // var text = await dataAsString.Content.ReadAsStringAsync();

            // var result = System.Text.Json.Serialization.JsonSerializer.Parse<OperationResult<UserData>>(text);

//             return new WeatherForecast[]{
// new WeatherForecast{
//     Summary = result.Result.Id,
//     Date = DateTime.Today,
//     TemperatureC = 33,
//     TemperatureF = 66
// }

//         };

return new WeatherForecast[]{
new WeatherForecast{
    Summary = Tsukinia.Core.PushNotification.PushNotificationManager.ReceivedNotifications.Count.ToString(),
    Date = DateTime.Today,
    TemperatureC = 33,
    TemperatureF = 66
}

        };

        }
    }
}
