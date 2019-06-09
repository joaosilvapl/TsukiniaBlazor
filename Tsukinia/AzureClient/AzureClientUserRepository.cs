using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Tsukinia;
using Tsukinia.Core;

namespace Tsukinia.AzureClient
{
    public class AzureClientUserRepository
    {
        public DataOperationResult<UserData> Insert(UserData userData)
        {
            var url = Secrets.AzureBaseUrl + "AddUserFunction";

            var body = System.Text.Json.Serialization.JsonSerializer.ToString(new
            {
                userName = userData.Name,
                userTypeId = (int)userData.Type,
                parentEmail = userData.ParentEmail,
            });

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

                return System.Text.Json.Serialization.JsonSerializer.Parse<DataOperationResult<UserData>>(response);
            }
        }

        public async Task<DataOperationResult<UserData>> Get(string parentEmail, string userName)
        {
            return await AzureClientHelper.GetData<DataOperationResult<UserData>>(
                $"GetUserFunction?userName={userName}&parentEmail={parentEmail}");
        }

        public async Task<DataOperationResult<UserData>> GetByUserId(string userId)
        {
            return await AzureClientHelper.GetData<DataOperationResult<UserData>>(
                $"GetUserByIdFunction?userId={userId.ToString()}");
        }

        public async Task<DataOperationResult<List<UserData>>> GetFamilyUsers(string familyId, UserType? userType)
        {
            return await AzureClientHelper.GetData<DataOperationResult<List<UserData>>>(
                $"GetFamilyUsersFunction?familyId={familyId}&userType={userType.ToString()}");
        }
    }
}
