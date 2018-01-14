using TourBox.Models;
using TourBox.ViewModels;
using Xamarin.Forms;

namespace TourBox.Views
{
    public partial class ExibirLocalView : ContentPage
    {
        public ExibirLocalView(LocalModel localModel)
        {
            InitializeComponent();
            BindingContext = new ExibirLocalViewModel(localModel);
        }

        public ExibirLocalView()
        {
            InitializeComponent();
        }
    }
}