using TourBox.Models;
using TourBox.ViewModels;
using Xamarin.Forms;

namespace TourBox.Views
{
    public partial class DetailsView : ContentPage
    {
        public DetailsView(LocalModel localModel)
        {
            InitializeComponent();
            BindingContext = new DetailsViewModel(localModel);
        }
    }
}