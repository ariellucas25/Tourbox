// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace TourBox.Helpers
{

  public static class Settings
  {
        private static ISettings AppSettings => CrossSettings.Current;

        const string UserIdKey = "userid";
        static readonly string UserIdDefault = string.Empty;

        const string AuthTokenKey = "authtoken";
        static readonly string AuthTokenDefault = string.Empty;

        public static string AuthToken
        {
            get { return AppSettings.GetValueOrDefault<string>(AuthToken, AuthTokenDefault); }
            set { AppSettings.AddOrUpdateValue<string>(AuthToken, value); }
        }

        public static string UserId
        {
            get { return AppSettings.GetValueOrDefault<string>(UserIdKey, UserIdDefault); }
            set { AppSettings.AddOrUpdateValue<string>(UserIdKey, value); }
        }

        public static bool IsLoggedin => !string.IsNullOrWhiteSpace(UserId);
    }
}