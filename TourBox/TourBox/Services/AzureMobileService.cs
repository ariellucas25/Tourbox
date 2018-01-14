using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TourBox.Helpers;
using TourBox.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Xamarin.Forms;

namespace TourBox.Services
{
    public class AzureMobileService
    {
        private static AzureMobileService _instance;
        public static AzureMobileService Instance => _instance ?? (_instance = new AzureMobileService());

        private MobileServiceClient _client;
        private IMobileServiceSyncTable<LocalModel> _localModel;

        public static bool UseAuth { get; set; } = false;

        private AzureMobileService() { }

        private async Task Initialize()
        {
            if (_client?.SyncContext?.IsInitialized ?? false)
                return;

            var store = new MobileServiceSQLiteStore(AppConfig.DatabaseName);
            store.DefineTable<LocalModel>();

            _client = new MobileServiceClient(AppConfig.MobileAppUri);
            await _client.SyncContext.InitializeAsync(store);

            _localModel = _client.GetSyncTable<LocalModel>();

            if (!string.IsNullOrWhiteSpace(Settings.AuthToken) && !string.IsNullOrWhiteSpace(Settings.UserId))
            {
                _client.CurrentUser = new MobileServiceUser(Settings.UserId)
                {
                    MobileServiceAuthenticationToken = Settings.AuthToken

                };
            }
        }

        public async Task<bool> LoginAsync()
        {
            Initialize();

            var auth = DependencyService.Get<IAuthentication>();
            var user = await auth.LoginAsync(_client, MobileServiceAuthenticationProvider.Facebook);

            if (user == null)
            {
                Settings.AuthToken = string.Empty;
                Settings.UserId = string.Empty;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Ops!", "Não conseguimos efetuar seu login, tente novamente.", "OK");
                });
                return false;
            }
            else
            {
                Settings.AuthToken = user.MobileServiceAuthenticationToken;
                Settings.UserId = user.UserId;
            }
            return true;
        }

   


        private async Task SyncLocais()
        {
            try
            {
                await _client.SyncContext.PushAsync();
                await _localModel.PullAsync("locais", _localModel.CreateQuery());
            }
            catch (Exception e)
            {
                LogHelper.Instance.AddLog(e);
            }
        }

        public async Task<IList<LocalModel>> GetLocais()
        {
            await Initialize();
            await SyncLocais();
            return await _localModel.OrderBy(a => a.Nome).ToListAsync();
        }

        public async Task SaveLocal(LocalModel localModel)
        {
            await Initialize();

            if (string.IsNullOrEmpty(localModel.Id))
                await _localModel.InsertAsync(localModel);
            else
                await _localModel.UpdateAsync(localModel);

            await SyncLocais();
        }

        public async Task DeleteLocal(LocalModel localModel)
        {
            await Initialize();
            await _localModel.DeleteAsync(localModel);
            await SyncLocais();
        }
    }
}
