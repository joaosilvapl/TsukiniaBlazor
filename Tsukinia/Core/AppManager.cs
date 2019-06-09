using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using Cloudcrate.AspNetCore.Blazor.Browser.Storage;
using Tsukinia.AzureClient;

namespace Tsukinia.Core
{
    public class AppManager
    {
        private const string SavedAppDataStorageKey = "SavedAppData";
        private static LocalStorage Storage = new LocalStorage();

        public AppData AppData { get; } = new AppData();

        public CultureInfo CurrentCulture = new CultureInfo(1033);

        //Singleton
        public static AppManager Instance { get; } = new AppManager();

        private AppManager()
        {
            this.LoadDataFromFile();
        }

        private void LoadDataFromFile()
        {
            var savedData = LoadSavedData();

            var userId = savedData?.UserId;
            this.CurrentCulture = new CultureInfo(savedData?.CultureInfoLcid ?? 1033);
            this.AppData.CurrencySymbol = savedData?.CurrencySymbol ?? GlobalConstants.DefaultCurrencySymbol;

            this.LoadUserById(userId);
        }

        private static SavedAppData LoadSavedData()
        {
            try
            {
                var savedData = Storage[SavedAppDataStorageKey];

                return JsonSerializer.Parse<SavedAppData>(savedData);

            }
            catch (Exception)
            {
                //TODO: log exception
                return null;
            }
        }

        public void SaveData()
        {
            var dataToSave = new SavedAppData
            {
                UserId = this.AppData.AppState == AppState.LoggedIn ? this.AppData.UserData.Id : null,
                CultureInfoLcid = this.CurrentCulture.LCID,
                CurrencySymbol = this.AppData.CurrencySymbol
            };

            var dataToSaveJson = System.Text.Json.Serialization.JsonSerializer.ToString(dataToSave);

            Storage[SavedAppDataStorageKey] = dataToSaveJson;
        }

        private static string GetFilePath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(path, "AppData");
        }

        public void RefreshAppData()
        {
            this.LoadUserById(this.AppData.UserData.Id);
        }

        public async void LoadUserById(string userId)
        {

            UserData userData = null;
            UserData childData = null;
            List<UserActivityData> childActivities = null;

            if (userId != null)
            {
                var userRepository = new AzureClientUserRepository();

                var getUserResult = await userRepository.GetByUserId(userId);

                //TODO: handle failure, add logging
                if (getUserResult.Success && getUserResult.Result != null)
                {
                    userData = getUserResult.Result;

                    if(userData.Type == UserType.Parent)
                    {
                        var getChildrenResult = await userRepository.GetFamilyUsers(userData.FamilyId, UserType.Child);
                    
                        if (getChildrenResult.Success && getChildrenResult.Result != null)
                        {
                            childData = getChildrenResult.Result.FirstOrDefault();
                        }
                    }
                    else
                    {
                        childData = userData;
                    }

                    if (childData != null)
                    {
                        var getChildActivitiesResult = await new AzureClientUserActivityRepository().GetAllForUser(childData.Id);

                        childActivities = getChildActivitiesResult.Success ? getChildActivitiesResult.Result : null;
                    }
                }
            }

            this.AppData.UserData = userData;
            this.AppData.ChildData = childData;
            this.AppData.ChildActivities = childActivities;
            this.AppData.AppState = userData != null ? AppState.LoggedIn : AppState.NotLoggedIn;
        }
    }
}
