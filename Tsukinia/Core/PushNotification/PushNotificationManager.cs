using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;

namespace Tsukinia.Core.PushNotification
{
    public class PushNotificationManager
    {
        public static List<string> ReceivedNotifications = new List<string>();

        public OperationResult<PushNotificationResponseData> Send(PushNotificationData pushNotificationData)
        {
            var notificationBody =
                $"{{\"notification_content\": {{\"name\": \"{pushNotificationData.Name}\",    \"title\": \"{pushNotificationData.Title}\",\"body\": \"{pushNotificationData.Body}\"}}}}";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(Tsukinia.Secrets.AppCenterPushNotificationApiUrl);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Headers["X-API-Token"] = Tsukinia.Secrets.AppCenterPushNotificationApiToken;
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(notificationBody);
                streamWriter.Flush();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            var responseStream = httpResponse.GetResponseStream();

            if (responseStream == null)
            {
                return new OperationResult<PushNotificationResponseData>
                {
                    Success = false,
                    ErrorMessage = "Response stream is null"
                };
            }

            using (var streamReader = new StreamReader(responseStream))
            {
                var response = streamReader.ReadToEnd();

                return new OperationResult<PushNotificationResponseData>
                {
                    Success = true,
                    Result = new PushNotificationResponseData
                    {
                        Message = response
                    }
                };
            }
        }
    }
}