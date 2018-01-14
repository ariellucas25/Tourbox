using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using TourBox.Helpers;
using TourBox.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(SocialAuthentication))]
namespace TourBox.Droid
{
    public class SocialAuthentication : IAuthentication
    {
        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client, 
            MobileServiceAuthenticationProvider provider, 
            IDictionary<string, string> parameters = null)
        {
            try
            {
                var user = await client.LoginAsync(Forms.Context, provider);

                Settings.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
                Settings.UserId = user?.UserId;

                return user;
            }
            catch (Exception ex)
            {
                //TODO: Log error
                throw;
            }
        }
    }
}