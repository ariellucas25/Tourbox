using System.Threading.Tasks;
using TourBox.Models;
using TourBox.Views;
using Xamarin.Forms;

namespace TourBox.Helpers
{
    public class NavigationHelper
    {
        private static NavigationHelper _instance;
        public static NavigationHelper Instance => _instance ?? (_instance = new NavigationHelper());

        private NavigationHelper() { }

        private NavigationPage GetMainPage()
        {
            return Application.Current.MainPage as NavigationPage;
        }

        public async Task GotoLocal(LocalModel localModel)
        {
            await GetMainPage().PushAsync(new ExibirLocalView(localModel));
        }

        public async Task GotoDetails(LocalModel localModel)
        {
            await GetMainPage().PushAsync(new DetailsView(localModel));
        }

        public async Task GoBack()
        {
            await GetMainPage().PopAsync();
        }

        public async Task GoToLocaisPage()
        {
            await Instance.GetMainPage().PushAsync(new LocaisView());
        }

    }
}
