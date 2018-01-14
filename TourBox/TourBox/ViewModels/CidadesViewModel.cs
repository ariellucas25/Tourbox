using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using TourBox.Helpers;
using TourBox.Views;
using Xamarin.Forms;

namespace TourBox.ViewModels
{
    public class CidadesViewModel : BaseViewModel
    {
        ICommand tapCommand;

        public CidadesViewModel()
        {
            tapCommand = new Command(OnTappedAsync);
        }

        public ICommand TapCommand
        {
            get { return tapCommand; }
        }

        public async void OnTappedAsync(object s)
        {
            await ShowToLocais();
        }

        private async Task ShowToLocais()
        {
            await NavigationHelper.Instance.GoToLocaisPage();
        }



    }
}


   
