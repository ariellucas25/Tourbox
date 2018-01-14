using TourBox.Helpers;
using TourBox.Models;
using TourBox.ViewModels;
using Xamarin.Forms;
namespace TourBox.Views
{
    
    public partial class LocaisView : ContentPage
    {

        private LocaisViewModel _locaisViewModel = new LocaisViewModel();
        public LocaisView()
        {
            InitializeComponent();
            BindingContext = _locaisViewModel;
        }



        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            (sender as ListView).SelectedItem = e;
            var localModel = e.Item as LocalModel;
            await NavigationHelper.Instance.GotoLocal(localModel);
        }
    }
}