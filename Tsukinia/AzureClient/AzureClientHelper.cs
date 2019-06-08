using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Tsukinia.AzureClient
{
    static class AzureClientHelper
    {
        public static async Task<T> GetData<T>(string functionNameAndParameters)
        {
            var url = Secrets.AzureBaseUrl + functionNameAndParameters;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Headers["x-functions-key"] = Secrets.AzureFunctionKey;
            httpWebRequest.Method = "GET";

            var httpResponse = await httpWebRequest.GetResponseAsync();

            var responseStream = httpResponse.GetResponseStream();

            if (responseStream == null)
            {
                throw new ApplicationException("Response stream is null");
            }

            using (var streamReader = new StreamReader(responseStream))
            {
                var response = await streamReader.ReadToEndAsync();

                return System.Text.Json.Serialization.JsonSerializer.Parse<T>(response);
            }
        }

        public static T PostData<T>(string functionNameAndParameters, object bodyData)
        {
            var url = Secrets.AzureBaseUrl + functionNameAndParameters;

            var body = System.Text.Json.Serialization.JsonSerializer.ToString(bodyData);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Headers["x-functions-key"] = Secrets.AzureFunctionKey;
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(body);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            var responseStream = httpResponse.GetResponseStream();

            if (responseStream == null)
            {
                throw new ApplicationException("Response stream is null");
            }

            using (var streamReader = new StreamReader(responseStream))
            {
                var response = streamReader.ReadToEnd();

                return System.Text.Json.Serialization.JsonSerializer.Parse<T>(response);
            }
        }
    }
}
