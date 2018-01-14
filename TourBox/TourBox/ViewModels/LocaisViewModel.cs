using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TourBox.Helpers;
using TourBox.Services;
using TourBox.Models;
using Xamarin.Forms;

namespace TourBox.ViewModels
{
    public class LocaisViewModel : BaseViewModel
    {
        public ObservableCollection<LocalModel> Locais { get; set; }

        public ICommand GetLocaisCommand
            => new Command(async () => await GetLocais());

        public ICommand AddLocalCommand
            => new Command(async () => await AddLocal());

        public LocaisViewModel()
        {
            Locais = new ObservableCollection<LocalModel>();
            GetLocaisCommand.Execute(null);
        }

        public async Task GetLocais()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                Locais.Clear();

                var items = await AzureMobileService.Instance.GetLocais();
                foreach (var item in items)
                    Locais.Add(item);
            }
            catch (Exception e)
            {
                LogHelper.Instance.AddLog(e);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task AddLocal()
        {
            await NavigationHelper.Instance.GotoDetails(new LocalModel());
        }
    }
}
