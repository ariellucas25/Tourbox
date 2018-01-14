using TourBox.ViewModels;
using Xamarin.Forms;

namespace TourBox.Views
{
    public partial class CidadesView : ContentPage
    {
        public CidadesView()
        {
            InitializeComponent();
            BindingContext = new CidadesViewModel();
        }
    }
}


