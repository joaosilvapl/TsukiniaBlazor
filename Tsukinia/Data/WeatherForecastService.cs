using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

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
var url = "https://taskrewardfunctions.azurewebsites.net/api/GetUserByIdFunction?userId=47b63af8-2c2e-4fe8-a438-ff221fbce180&code=[function code here]";
        
        HttpClient client = new HttpClient();
        
        var dataAsString = await client.GetAsync(url);

var text = await dataAsString.Content.ReadAsStringAsync();

            var result = System.Text.Json.Serialization.JsonSerializer.Parse<OperationResult>(text);

        return new WeatherForecast[]{
new WeatherForecast{
    Summary = result.Result.Id,
    Date = DateTime.Today,
    TemperatureC = 33,
    TemperatureF = 66
}

        };

        }
    }
}
